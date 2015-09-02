using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using CommerceTraining.Models.Pages;
using CommerceTraining.Models.ViewModels;
using EPiServer.Commerce.Catalog.ContentTypes;
using Mediachase.Commerce.Website.Search;
using Mediachase.Search.Extensions;
using Mediachase.Search;
using Mediachase.Commerce.Catalog.Objects;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Catalog.Managers;
using System;
using EPiServer.ServiceLocation;
using CommerceTraining.Models.Catalog;
using EPiServer.Web.Routing;

namespace CommerceTraining.Controllers
{
    public class SearchController : PageController<SearchPage>
    {
        public IEnumerable<IContent> localContent { get; set; }
        public readonly IContentLoader _contentLoader;
        public readonly ReferenceConverter _referenceConverter;
        public readonly UrlResolver _urlResolver;

        public SearchController(IContentLoader contentLoader, ReferenceConverter referenceConverter, UrlResolver urlResolver)
        {
            _contentLoader = contentLoader;
            _referenceConverter = referenceConverter;
            _urlResolver = urlResolver;
        }

        public ActionResult Index(SearchPage currentPage)
        {
            var model = new SearchPageViewModel
            {
                CurrentPage = currentPage,
            };

            return View(model);
        }

        public ActionResult Search(string keyWord)
        {
            // ToDo: SearchHelper and Criteria 
            SearchFilterHelper searchHelper = SearchFilterHelper.Current; // the easy way

            CatalogEntrySearchCriteria criteria = searchHelper.CreateSearchCriteria(keyWord
                , CatalogEntrySearchCriteria.DefaultSortOrder);

            criteria.RecordsToRetrieve = 25;
            criteria.StartingRecord = 0;
            criteria.Locale = "en"; // needed

            int count = 0; // "Out"
            bool cacheResult = true;
            TimeSpan timeSpan = new TimeSpan(0, 10, 0);

            // ToDo: Search 
            // One way of "doing it" ... retrieve it like ISearchResults (preferred, most certainly)
            ISearchResults searchResult = searchHelper.SearchEntries(criteria);
            ISearchDocument aDoc = searchResult.Documents.FirstOrDefault();
            int[] ints = searchResult.GetKeyFieldValues<int>();

            // ECF style Entries, old-school, not recommended... if not working with DTOs or the ContentModel
            Entries entries = CatalogContext.Current.GetCatalogEntries(ints // Note "ints"
                , new CatalogEntryResponseGroup(CatalogEntryResponseGroup.ResponseGroup.CatalogEntryInfo));

            // Same thing ECF, old-style, not recommended... if not absolutely needed...
            // Use the helper and get the entries direct 
            // If entries are needed ... like for calculating discounts with StoreHelper()
            Entries entriesDirect = searchHelper.SearchEntries(criteria, out count // Note the different return-types ... akward!
                , new CatalogEntryResponseGroup(CatalogEntryResponseGroup.ResponseGroup.CatalogEntryInfo)
                , false, new TimeSpan());

            // CMS style (better)... using ReferenceConverter and ContentLoader 
            List<ContentReference> refs = new List<ContentReference>();
            ints.ToList().ForEach(i => refs.Add(_referenceConverter.GetContentLink(i, 0)));

            // LoaderOptions() is new in CMS 8
            // ILanguageSelector selector = ServiceLocator.Current.GetInstance<ILanguageSelector>(); // obsolete
            localContent = _contentLoader.GetItems(refs, new LoaderOptions()); // use this in CMS 8+


            // ToDo: Facets
            List<string> facetList = new List<string>();

            int facetGroups = searchResult.FacetGroups.Count();
            foreach (ISearchFacetGroup item in searchResult.FacetGroups)
            {
                foreach (var item2 in item.Facets)
                {
                    facetList.Add(String.Format("{0} {1} ({2})", item.Name, item2.Name, item2.Count));
                }
            }

            // ToDo: As a last step - un-comment and fill up the ViewModel
            var searchResultViewModel = new SearchResultViewModel();

            searchResultViewModel.totalHits = new List<string> { "" }; // change
            searchResultViewModel.nodes = localContent.OfType<FashionNode>();
            searchResultViewModel.products = localContent.OfType<FashionProduct>();
            searchResultViewModel.variants = localContent.OfType<FashionVariation>(); 
            searchResultViewModel.allContent = localContent;
            searchResultViewModel.facets = facetList;


            return View(searchResultViewModel);
        }
    }
}