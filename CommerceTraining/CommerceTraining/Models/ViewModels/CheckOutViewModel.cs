namespace CommerceTraining.Models.Pages
{
    public class CheckOutViewModel
    {
        
        public CheckOutPage CurrentPage { get; set; }

        // Lab E1 - create properties below

        
        public CheckOutViewModel()
        { }

        public CheckOutViewModel(CheckOutPage currentPage)
        {
            CurrentPage = currentPage;
        }


    }
}