using ControlzEx.Standard;
using MahApps.Metro.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using wf_final.Models;
using wf_final.Logics;
using MySql.Data.MySqlClient;
using System.Data;
using MySql.Data.Types;
using Org.BouncyCastle.Utilities.Collections;

namespace wf_final
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        bool IsFavorite = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string api_key = "4n4Miwzm5p37SLb9Jk9bJa%2FMhFYSTl8mkQIensYxsOuwWyjpePzkk6oyRp3pOsd8GVnzwwQelKHMwSc0bPVfSA%3D%3D";
            string openApiUri = $@"https://apis.data.go.kr/6260000/BusanCultureMusicalService/getBusanCultureMusical?serviceKey={api_key}&pageNo=1&numOfRows=1000&resultType=json";
            string result = string.Empty;

            WebRequest req = null;
            WebResponse res = null;
            StreamReader reader = null;

            try
            {
                req = WebRequest.Create(openApiUri); // URL을 넣어서 객체를 생성
                res = await req.GetResponseAsync();// 요청한 결과를 비동기 응답에 할당
                reader = new StreamReader(res.GetResponseStream());
                result = reader.ReadToEnd(); // json결과 텍스트로 저장

                Debug.WriteLine(result);
            }
            catch (Exception ex)
            {
                await Commons.ShowMessageAsync("오류", $"OpenAPI 조회 오류 : {ex.Message}");
            }
            finally
            {
                reader.Close();
                res.Close();
            }
            var jsonResult = JObject.Parse(result);
            var totalCount = Convert.ToInt32(jsonResult["getBusanCultureMusical"]["totalCount"]);
            var resultCode = Convert.ToInt32(jsonResult["getBusanCultureMusical"]["header"]["code"]);

            try
            {
                if (resultCode == 00)
                {
                    var item = jsonResult["getBusanCultureMusical"]["item"];
                    var json_Array = item as JArray;
                    
                    var musicals = new List<Musicals>();
                    foreach (var musical in json_Array)
                    {
                        musicals.Add(new Musicals
                        {
                            Id = Convert.ToInt32(musical["id"]),
                            Op_at = Convert.ToString(musical["op_at"]),
                            Op_st_dt = Convert.ToDateTime(musical["op_st_dt"]),
                            Op_ed_dt = Convert.ToDateTime(musical["op_ed_dt"]),
                            Pay_at = Convert.ToString(musical["pay_at"]),
                            Place_nm = Convert.ToString(musical["place_nm"]),
                            Title = Convert.ToString(musical["title"]),
                            Res_no = Convert.ToInt32(musical["res_no"]),
                        });
                    }
                    this.DataContext = musicals;
                    StsResult.Content = $"부산광역시 뮤지컬 총 {musicals.Count} 개   |  2020년도 이후 ";
                }
            }
            catch (Exception ex) 
            {
                await Commons.ShowMessageAsync("오류", $"JSON 조회 오류 : {ex.Message}");
            }
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if(GrdResult.Items.Count==0)
            {
                await Commons.ShowMessageAsync("오류", "저장할 데이터가 조회되지 않았습니다.");
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(Commons.myConnString))
                {
                    if (conn.State == System.Data.ConnectionState.Closed) conn.Open();

                    var query = @"INSERT INTO musicals
                                                                (Id,
                                                                Op_at,
                                                                Op_st_dt,
                                                                Op_ed_dt,
                                                                Pay_at,
                                                                Place_nm,
                                                                Title,
                                                                Res_no)
                                                                VALUES
                                                                (@Id,
                                                                @Op_at ,
                                                                @Op_st_dt ,
                                                                @Op_ed_dt ,
                                                                @Pay_at ,
                                                                @Place_nm ,
                                                                @Title ,
                                                                @Res_no);";
                    var insRes = 0;
                    foreach(Musicals temp in GrdResult.SelectedItems) 
                    {
                        if(temp is Musicals)
                        {
                            var item = temp;

                            MySqlCommand cmd = new MySqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@Id", item.Id);
                            cmd.Parameters.AddWithValue("@Op_at", item.Op_at);
                            cmd.Parameters.AddWithValue("@Op_st_dt", item.Op_st_dt);
                            cmd.Parameters.AddWithValue("@Op_ed_dt", item.Op_ed_dt);
                            cmd.Parameters.AddWithValue("@Pay_at", item.Pay_at);
                            cmd.Parameters.AddWithValue("@Place_nm", item.Place_nm);
                            cmd.Parameters.AddWithValue("@Title", item.Title);
                            cmd.Parameters.AddWithValue("@Res_no", item.Res_no);

                            insRes += cmd.ExecuteNonQuery(); 
                        }
                    }
                    await Commons.ShowMessageAsync("저장", "DB 저장 성공");
                    StsResult.Content = $"{insRes} 건 저장 ";
                 }
            }
            catch (Exception ex) 
            {
                await Commons.ShowMessageAsync("오류", $"DB 저장 오류 {ex.Message}");
            }
        }

        private async void BtnShow_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = null;
            List<Musicals> list = new List<Musicals>();   

            try
            {
                using(MySqlConnection conn = new MySqlConnection(Commons.myConnString))
                {
                    if (conn.State == System.Data.ConnectionState.Closed) conn.Open();

                    var query = @"SELECT Id,
                                                        Op_at,
                                                        Op_st_dt,
                                                        Op_ed_dt,
                                                        Pay_at,
                                                        Place_nm,
                                                        Title,
                                                        Res_no
                                                    FROM musicals
                                                ORDER BY Id ASC";

                    var cmd = new MySqlCommand(query, conn);
                    var adapter = new MySqlDataAdapter(cmd);
                    var dSet = new DataSet();
                    adapter.Fill(dSet, "Musicals");

                    foreach (DataRow dr in dSet.Tables["Musicals"].Rows)
                    {
                        list.Add(new Musicals
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Op_at = Convert.ToString(dr["Op_at"]),
                            Op_st_dt = Convert.ToDateTime(dr["Op_st_dt"]),
                            Op_ed_dt = Convert.ToDateTime(dr["Op_ed_dt"]),
                            Pay_at = Convert.ToString(dr["Pay_at"]),
                            Place_nm = Convert.ToString(dr["Place_nm"]),
                            Title = Convert.ToString(dr["Title"]),
                            Res_no = Convert.ToInt32(dr["Res_no"]),
                        });
                    }
                    this.DataContext = list;
                    IsFavorite = true;
                }
            }
            catch (Exception ex) 
            {
                await Commons.ShowMessageAsync("오류", $"DB조회 오류 {ex.Message}");

            }
        }
        private async void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (DtpPerformDate.SelectedDate != null)
                {
                    //MessageBox.Show(DtpPerformDate.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    string Picked = DtpPerformDate.SelectedDate.Value.ToString("yyyy-MM-dd");
                    using (MySqlConnection conn = new MySqlConnection(Commons.myConnString))
                    {
                        conn.Open();

                        var query = $@"SELECT * FROM miniproject.musicals 
                                 WHERE date_format(op_st_dt, '%Y-%m-%d') >= @Picked
                               ORDER BY Op_st_dt;";

                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Picked", Picked);
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        List<Musicals> list = new List<Musicals>();
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            list.Add(new Musicals
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Op_at = Convert.ToString(dr["Op_at"]),
                                Op_st_dt = Convert.ToDateTime(dr["Op_st_dt"]),
                                Op_ed_dt = Convert.ToDateTime(dr["Op_ed_dt"]),
                                Pay_at = Convert.ToString(dr["Pay_at"]),
                                Place_nm = Convert.ToString(dr["Place_nm"]),
                                Title = Convert.ToString(dr["Title"]),
                                Res_no = Convert.ToInt32(dr["Res_no"]),
                            });
                        }
                        this.DataContext = list;
                        IsFavorite = true;
                    }
                }
            }
            catch (Exception ex)
            {
                await Commons.ShowMessageAsync("오류", $" 날짜 조회 오류 {ex.Message}");
            }

        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if(GrdResult.Items.Count == 0) 
            {
                await Commons.ShowMessageAsync("오류", "삭제할 데이터를 선택하세요.");
                return;
            }
            try
            {
                using(MySqlConnection conn = new MySqlConnection(Commons.myConnString))
                {
                    if(conn.State == ConnectionState.Closed) conn.Open();
                    
                    var query = " DELETE FROM musicals WHERE Id = @Id";
                    var delRes = 0;

                    foreach(Musicals item in GrdResult.SelectedItems)
                    {
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Id", item.Id);

                        delRes += cmd.ExecuteNonQuery();
                    }

                    if(delRes==GrdResult.SelectedItems.Count)
                    {
                        await Commons.ShowMessageAsync("삭제", "DB 삭제 성공");
                        StsResult.Content = $" {delRes} 건 삭제 완료";  // 화면에 안나옴 
                    }
                }
            }
            catch (Exception ex) 
            {
                await Commons.ShowMessageAsync("오류", $"DB삭제 오류 {ex.Message}");
            }
        }
    }
}
