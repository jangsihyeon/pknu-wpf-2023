using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wp12_finedustCheck.Models
{
    public class DustSensor
    {
        /*"dev_id": "BAAH02000129",
      "name": "대기질센서9",
      "loc": "경남 김해시 식만로 388 불암동사무소",
      "coordx": "128.926799",
      "coordy": "35.222096",
      "ison": true,
      "pm10_after": 18,
      "pm25_after": 12,
      "state": 0,
      "timestamp": "2023-04-27 13:57:49.586",
      "company_id": "9b0db4061a9841608c03f011dd7dba23",
      "company_name": "기후대기과"*/

        public int Id { get; set; }
        public string Dev_id { get; set; }
        public string Name { get; set; }
        public string Loc { get; set; }
        public double Coordx { get; set; }
        public double Coordy { get; set; }
        public bool Ison { get; set; }
        public int Pm10_after { get; set; }
        public int Pm25_after { get; set; }
        public int State { get; set; }
        public DateTime Timestamp { get; set; }
        public string Company_id { get; set; }
        public string Company_name { get; set;}
    }
}


