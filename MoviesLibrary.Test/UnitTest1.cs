namespace MoviesLibrary.Test
{
    [TestClass]
    public class UnitTest1
    {
        readonly HttpClient _httpClient;
        readonly MovieService _movieService;
        public UnitTest1()
        {
            _httpClient = new HttpClient();
            _movieService = new MovieService(_httpClient);
        }
        [TestMethod]
        public void TestMethodSearch()
        {
            var name = "÷Ì÷Ìœ¿";
            var result = _movieService.Search(name);
            Assert.IsTrue(result.Any());
        }

        public void TestMethodGetDetail(int cat, string EntId)
        {
            var result = _movieService.GetDetail(cat, EntId);
            Assert.IsNotNull(result);
        }
    }
}