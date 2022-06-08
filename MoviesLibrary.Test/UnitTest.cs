using System.Text.Json;
using MoviesLibrary.Common.Enum;
using MoviesLibrary.Model;
using System.Text.RegularExpressions;

namespace MoviesLibrary.Test
{
    [TestClass]
    public class UnitTest
    {
        readonly MovieService _movieService;
        readonly List<string> _listItems;

        public UnitTest()
        {
            _movieService = new MovieService();
            _listItems = new List<string> { "斗罗大陆" };
        }

        [TestMethod("搜索测试"), Priority(1)]
        public void TestMethodSearch()
        {
            foreach (var item in _listItems)
            {
                var result = _movieService.Search(item);
                if (result.Any())
                {
                    Assert.IsTrue(true, $"获取“{item}”数据成功！");
                    var tops = _movieService.GetTops(x => x.Cat == CatType.电视剧);
                    Assert.IsTrue(tops.Any(), "获取排行榜数据失败！");
                }
                else
                {
                    Assert.IsTrue(false, $"获取“{item}”数据失败！");
                }
            }
        }

        [TestMethod("获取详情"), Priority(2)]
        public void TestMethodDetail_1()
        {
            string entId = "faXpYRH6Rnb4UR";
            var detail = _movieService.GetDetail(CatType.电影, entId);
            Assert.IsTrue(detail.EntId == entId);
        }

        [TestMethod("获取详情"), Priority(2), TestCategory("获取指定页面范围的数据")]
        public void TestMethodDetail_2()
        {
            //_ = int.TryParse(Regex.Replace($"{obj.CoverInfo?.FirstOrDefault().Value}", @"[^0-9]+", string.Empty), out int total);
            //_ = Enum.TryParse(obj.PlayLinks!.FirstOrDefault().Key, out PlayLinkType linkType);
            string entId = "Q4RraX7lTzbqN3";
            var detail = _movieService.GetDetail(CatType.电视剧, entId, 1, 20, PlayLinkType.imgo);
            Assert.IsTrue(detail.EntId == entId);
        }
    }
}