using AutoMapper;
using jQueryDatatable;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using WP.Common;
using WP.DataContext;
using WP.EDTOs;
using WP.EDTOs.Categories;
using WP.Repository;

namespace WP.Service.Categories
{
    public interface ITermsService : IDisposable
    {
        Task<ResponseDto<List<CATEGORIES_TERMS_DTO>>> GetCategories(ulong postid = 0, ulong matchingid = 0);
		Task<ResponseDto<List<CATEGORIES_TERMS_DTO>>> GetAllCategories(ulong postid = 0, ulong matchingid = 0);
		Task<ResponseDto<List<TAGS_TERMS_DTO>>> GetTags(ulong postid = 0, ulong matchingid = 0);
        Task<ResponseDto<TAGS_TERMS_DTO>> GetTag(ulong tagid);
        /// <summary>
        /// Add new category
        /// </summary>
        /// <param name="catname"></param>
        /// <param name="parentcat"></param>
        /// <returns></returns>
        Task<ResponseDto<bool>> AddCateroty(string catname, ulong parentcat = 0);
        /// <summary>
        /// Add or Removed cat or tag from the db as user selection
        /// </summary>
        /// <param name="postid"></param>
        /// <param name="tag_cat_id"></param>
        /// <returns></returns>
        Task<ResponseDto<bool>> AssignRemoved_Category_To_Post(ulong postid, ulong[] cat_id);
        /// <summary>
        /// Add or Removed cat or tag from the db as user selection
        /// </summary>
        /// <param name="postid"></param>
        /// <param name="tag_cat_id"></param>
        /// <returns></returns>
        Task<ResponseDto<bool>> AssignRemoved_Tag_To_Post(ulong postid, ulong[] tag_id);
        Task<ResponseDto<bool>> Delete_TermTaxonomy(ulong[] termtaxonomyids);
        Task<ResponseDto<Datatable<TERM_SP_RESPONSE>>> GetTagsPaged(TermsPagingRequest reqDto);
        Task<ResponseDto<Datatable<TERM_SP_RESPONSE>>> GetCategoriesPaged(TermsPagingRequest reqDto);
        Task<ResponseDto<List<ParentCategoryDto>>> GetParentCategories();

    }
    public class TermsService : BaseServices, ITermsService
    {
        #region private
        private readonly IRepository<WpTerm> _repoTerm;
        private readonly IRepository<WpTermRelationship> _repoTermRelation;
        private readonly IRepository<WpTermTaxonomy> _repoTermTaxonomy;
        private readonly IMapper _mapper;
        #endregion

        #region ctor
        public TermsService(IRepository<WpTerm> repoTerm, IRepository<WpTermRelationship> repoTermRelation, IRepository<WpTermTaxonomy> repoTermTaxonomy,
            IMapper mapper)
        {
            _repoTerm = repoTerm;
            _repoTermRelation = repoTermRelation;
            _repoTermTaxonomy = repoTermTaxonomy;
            _mapper = mapper;
        }
        #endregion

        #region functions
        public async Task<ResponseDto<List<CATEGORIES_TERMS_DTO>>> GetCategories(ulong postid = 0, ulong matchingid = 0)
        {
            try
            {
                var query = $"CALL GET_CATEGORIES_TERMS({postid},{matchingid})";
                var jsonsResult = _repoTerm.Db.Database.SqlQueryRaw<CATEGORIES_TERMS_DTO>(query).ToList();
                var data = jsonsResult.Select(r => _mapper.Map<CATEGORIES_TERMS_DTO>(r));
                var ndata = data.Where(s => (s.Parent_Id ?? 0) == 0).Select(s =>
                {
                    s.Subcategory = data.Where(r => r.Parent_Id == s.Term_Id).ToList();
                    return s;
                }).ToList();


                return await Task.FromResult(new SuccessResponseDto<List<CATEGORIES_TERMS_DTO>>(ndata));
            }
            catch (Exception ex)
            {

                return await Task.FromResult(new FailedResponseDto<List<CATEGORIES_TERMS_DTO>>(ex.GetActualError()));
            }
        }
		public async Task<ResponseDto<List<CATEGORIES_TERMS_DTO>>> GetAllCategories(ulong postid = 0, ulong matchingid = 0)
		{
			try
			{
				var query = $"CALL GET_CATEGORIES_TERMS({postid},{matchingid})";
				var jsonsResult = _repoTerm.Db.Database.SqlQueryRaw<CATEGORIES_TERMS_DTO>(query).ToList();
				var data = jsonsResult.Select(r => _mapper.Map<CATEGORIES_TERMS_DTO>(r));


				return await Task.FromResult(new SuccessResponseDto<List<CATEGORIES_TERMS_DTO>>(data.ToList()));
			}
			catch (Exception ex)
			{

				return await Task.FromResult(new FailedResponseDto<List<CATEGORIES_TERMS_DTO>>(ex.GetActualError()));
			}
		}
		public async Task<ResponseDto<List<TAGS_TERMS_DTO>>> GetTags(ulong postid = 0,ulong matchingid = 0)
        {
            try
            {
                var query = $"CALL GET_TAGS_TERMS({postid}, {matchingid})";
                var jsonsResult = _repoTerm.Db.Database.SqlQueryRaw<TAGS_TERMS_DTO>(query).ToList();
                var data = jsonsResult.Select(r => _mapper.Map<TAGS_TERMS_DTO>(r)).ToList();
                return await Task.FromResult(new SuccessResponseDto<List<TAGS_TERMS_DTO>>(data));
            }
            catch (Exception ex)
            {

                return await Task.FromResult(new FailedResponseDto<List<TAGS_TERMS_DTO>>(ex.GetActualError()));
            }
        }

        public async Task<ResponseDto<bool>> AssignRemoved_Category_To_Post(ulong postid, ulong[] cat_id)
        {
            // Get existing items from the repository
            var catids = _repoTermTaxonomy.GetAll(s => s.Taxonomy == "category").Select(s=>s.TermId);
            var existItems = _repoTermRelation.GetAll(s => catids.Contains(s.TermTaxonomyId) && s.ObjectId == postid);
            var tagcatid = existItems.Select(s => s.TermTaxonomyId).ToList();  // Existing items (from DB)
            var newItems = cat_id.ToList();  // Newly selected items (from UI)

            // Find removed items: These are the items that are in the DB but not in the new list
            var removedItems = tagcatid.Except(newItems).ToList();

            // Find newly selected items: These are the items that are in the new list but not in the DB
            var newlySelectedItems = newItems.Except(tagcatid).ToList();

            // Optionally, find the common items: These are the items that were already in the DB and are still selected
            var commonItems = tagcatid.Intersect(newItems).ToList();

            // Removed item which are unselted
            await Delete_TermTaxonomy(removedItems.ToArray());

            var items = newlySelectedItems.Select(s => new WpTermRelationship { ObjectId = postid, TermTaxonomyId = s });
            await _repoTermRelation.InsertAsync(items);

            return new SuccessResponseDto<bool>(true);

        }
        public async Task<ResponseDto<bool>> AssignRemoved_Tag_To_Post(ulong postid, ulong[] tag_id)
        {

            var catids = _repoTermTaxonomy.GetAll(s => s.Taxonomy == "post_tag").Select(s => s.TermId);
            var existItems = _repoTermRelation.GetAll(s => catids.Contains(s.TermTaxonomyId) && s.ObjectId == postid);
            var tagcatid = existItems.Select(s => s.TermTaxonomyId).ToList();  // Existing items (from DB)
            var newItems = tag_id.ToList();  // Newly selected items (from UI)

            // Find removed items: These are the items that are in the DB but not in the new list
            var removedItems = tagcatid.Except(newItems).ToList();

            // Find newly selected items: These are the items that are in the new list but not in the DB
            var newlySelectedItems = newItems.Except(tagcatid).ToList();

            // Optionally, find the common items: These are the items that were already in the DB and are still selected
            var commonItems = tagcatid.Intersect(newItems).ToList();

            await Delete_TermTaxonomy(removedItems.ToArray());
            var items = newlySelectedItems.Select(s => new WpTermRelationship { ObjectId = postid, TermTaxonomyId = s });
            await _repoTermRelation.InsertAsync(items);

            return new SuccessResponseDto<bool>(true);
        }
        
        public async Task<ResponseDto<bool>> AddCateroty(string catname, ulong parentcat = 0)
        {
            var term = new WpTerm
            {
                Name = catname,
                Slug = catname.ToLower().Replace(" ", "-"),
            };
            await _repoTerm.InsertAsync(term);

            var termTaxonomy = new WpTermTaxonomy
            {
                Description = "Its a category description",
                Parent = parentcat,
                Taxonomy = "category",
                TermId = term.TermId,
            };
            await _repoTermTaxonomy.InsertAsync(termTaxonomy);
            return new SuccessResponseDto<bool>(true);
        }
        #endregion
        public async Task<ResponseDto<bool>> Delete_TermTaxonomy(ulong[] termtaxonomyids)
        {
            try
            {
                if (termtaxonomyids.Length > 0)
                {
                    var query = $"CALL DELETE_TERMTAXONOMY('{string.Join(',', termtaxonomyids)}')";
                    var isdelte = _repoTermTaxonomy.Db.Database.ExecuteSqlRaw(query);
                }
                return new SuccessResponseDto<bool>(true);
            }
            catch (Exception ex)
            {
                return new FailedResponseDto<bool>(ex.GetActualError());
            }
        }
        public async Task<ResponseDto<TAGS_TERMS_DTO>> GetTag(ulong tagid)
        {
            try
            {
                var query = $"CALL GET_TAG_TERM_BY_ID({tagid})";
                var jsonsResult = _repoTerm.Db.Database.SqlQueryRaw<TAGS_TERMS_DTO>(query).SingleOrDefault();
                var data = _mapper.Map<TAGS_TERMS_DTO>(jsonsResult);
                return await Task.FromResult(new SuccessResponseDto<TAGS_TERMS_DTO>(data));
            }
            catch (Exception ex)
            {

                return await Task.FromResult(new FailedResponseDto<TAGS_TERMS_DTO>(ex.GetActualError()));
            }
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _repoTerm.Dispose();
        }

        public async Task<ResponseDto<Datatable<TERM_SP_RESPONSE>>> GetTagsPaged(TermsPagingRequest reqDto)
        {
            try
            {
                var query = "CALL GET_TAGS_PAGED(@page, @pageSize, @searchText)";

                var jsonsResult = _repoTerm.Db.Database.SqlQueryRaw<TERM_SP_RESPONSE>(
                    query,
                    new MySqlParameter("@page", reqDto.Page),
                    new MySqlParameter("@pageSize", reqDto.PageSize),
                    new MySqlParameter("@searchText", reqDto.SearchText ?? "")
                ).ToList();


                var data = jsonsResult.Select(r => _mapper.Map<TERM_SP_RESPONSE>(r));
                var output = new Datatable<TERM_SP_RESPONSE>(data, reqDto.Draw, jsonsResult.FirstOrDefault()?.TotalCount ?? 0, jsonsResult.FirstOrDefault()?.TotalCount ?? 0);

                return await Task.FromResult(new SuccessResponseDto<Datatable<TERM_SP_RESPONSE>>(output));
            }
                catch (Exception ex)
            {

                return await Task.FromResult(new FailedResponseDto<Datatable<TERM_SP_RESPONSE>>(ex.GetActualError()));
            }
        }
        public async Task<ResponseDto<Datatable<TERM_SP_RESPONSE>>> GetCategoriesPaged(TermsPagingRequest reqDto)
        {
            try
            {
                var query = "CALL GET_CATEGORIES_PAGED(@page, @pageSize, @searchText)";

                var jsonsResult = _repoTerm.Db.Database.SqlQueryRaw<TERM_SP_RESPONSE>(
                    query,
                    new MySqlParameter("@page", reqDto.Page),
                    new MySqlParameter("@pageSize", reqDto.PageSize),
                    new MySqlParameter("@searchText", reqDto.SearchText ?? "")
                ).ToList();


                var data = jsonsResult.Select(r => _mapper.Map<TERM_SP_RESPONSE>(r));
                var output = new Datatable<TERM_SP_RESPONSE>(data, reqDto.Draw, jsonsResult.FirstOrDefault()?.TotalCount ?? 0, jsonsResult.FirstOrDefault()?.TotalCount ?? 0);

                return await Task.FromResult(new SuccessResponseDto<Datatable<TERM_SP_RESPONSE>>(output));
            }
            catch (Exception ex)
            {

                return await Task.FromResult(new FailedResponseDto<Datatable<TERM_SP_RESPONSE>>(ex.GetActualError()));
            }
        }

        public async Task<ResponseDto<List<ParentCategoryDto>>> GetParentCategories()
        {
            try
            {
                var parentCategories = (from term in _repoTerm.Db.WpTerms
                                        join taxonomy in _repoTerm.Db.WpTermTaxonomies
                                        on term.TermId equals taxonomy.TermId
                                        where taxonomy.Parent == 0 && taxonomy.Taxonomy == "category"
                                        select new ParentCategoryDto
                                        {   
                                            TermId = taxonomy.TermTaxonomyId,
                                            Name = term.Name
                                        }).Distinct().ToList();


                return await Task.FromResult(new SuccessResponseDto<List<ParentCategoryDto>>(parentCategories));
            }
            catch (Exception ex)
            {

                return await Task.FromResult(new FailedResponseDto<List<ParentCategoryDto>>(ex.GetActualError()));
            }


        }
    }

}
