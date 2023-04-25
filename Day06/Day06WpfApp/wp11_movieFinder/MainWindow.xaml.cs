using MahApps.Metro.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wp11_movieFinder.Logics;
using wp11_movieFinder.Models;

namespace wp11_movieFinder
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void BtnNaverMovie_Click(object sender, RoutedEventArgs e)
        {
            await Commons.ShowMessageAsync("네이버 영화", "네이버 영화 사이트로 갑니다. ");
        }

        // 검색 버튼 , 네이버 API 검색
        private async void BtnSearchMovie_Click(object sender, RoutedEventArgs e)
        {
            // 입력 검증 
            if(string.IsNullOrEmpty(TxtMovieName.Text))
            {
                await Commons.ShowMessageAsync("검색", "검색할 영화명을 입력하세요.");
                return;
            }
           
            //if(TxtMovieName.Text.Length <= 2)
            //{
            //    await Commons.ShowMessageAsync("검색", "검색어를 2자 이상 입력하세요.");
            //    return;
            //}

            try
            {
                SearchMovie(TxtMovieName.Text);
            }
            catch(Exception ex) 
            {
                await Commons.ShowMessageAsync("오류", $"오류 발생 : {ex.Message}");
            }
        }

        // 텍스트 박스에서 키를 입력할때 엔터를 누르면 검색 시작 
        private void TxtMovieName_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key ==Key.Enter)
            {
                BtnSearchMovie_Click(sender, e);
            }
        }

        // 실제 검색 메소드
        private async void SearchMovie(string movieName)
        {
            string tmdb_apiKey = "741e0fea38883e5b97bc1c63154106a2";
            string encoding_movieName = HttpUtility.UrlEncode(movieName, Encoding.UTF8);
            string openApiUri = $@"https://api.themoviedb.org/3/search/movie?api_key={tmdb_apiKey}&language=ko-KR&page=1&include_adult=false&query={encoding_movieName}";   // 검색 URL
            string result = string.Empty;                   // 결과값 

            // API 실행할 객체 
            WebRequest req = null;
            WebResponse res = null;
            StreamReader reader = null;

            // 네이버 API 요청 
            try
            {
                req = WebRequest.Create(openApiUri);  // Url을 넣어서 객체를 생성
                res = req.GetResponse();  // 요청한 결과를 응답에 할당 
                reader = new StreamReader(res.GetResponseStream());
                result = reader.ReadToEnd();   // json 결과를 텍스트로 저장 

                Debug.WriteLine(result);
            }
            catch(Exception ex) 
            {
                throw ex;
            }
            finally
            {
                reader.Close();
                res.Close();
            }

            // result 를 json으로 
            var jsonResult = JObject.Parse(result);

            int total = Convert.ToInt32(jsonResult["total_results"]);  // 전체 검색 결과수

           //await Commons.ShowMessageAsync("검색결과", total.ToString());

            var items = jsonResult["results"];
            // items를 데이터 그리드에 표시 
            var json_array = items as JArray;

            var movieItems = new List<Movieitem>();  // json에서 넘어온 배열을 담을 장소 
            foreach(var val in json_array)
            {
                var MovieItem = new Movieitem()
                {
                    Id = Convert.ToInt32(val["id"]),
                    Title = Convert.ToString(val["title"]),
                    Original_Title = Convert.ToString(val["original_title"]),
                    Release_Date = Convert.ToString(val["release_data"]),
                    Vote_Average = Convert.ToDouble(val["vote_average"])
                    //Adult = Convert.ToBoolean(val["adult"])
                };
                movieItems.Add(MovieItem);
            }
            this.DataContext = movieItems;
        }
    }
}
