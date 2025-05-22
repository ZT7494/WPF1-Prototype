using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_testing
{
    
    //this is just class definitions for the JSON packages returned by the OpenF1 API
    class Driver
    {
        public required string broadcast_name { get; set; }
        public required string country_code { get; set; }
        public required int driver_number { get; set; }
        public required string first_name { get; set; }
        public required string full_name { get; set; }
        public required string headshot_url { get; set; }
        public required string last_name { get; set; }
        public required int meeting_key { get; set; }
        public required string name_acronym { get; set; }
        public required int session_key { get; set; }
        public required string team_colour { get; set; }
        public required string team_name { get; set; }


        public List<string> getAtts()
        {
            List<string> atts = new List<string>() { broadcast_name, country_code, driver_number.ToString(), first_name, full_name, headshot_url, last_name, meeting_key.ToString(), name_acronym, session_key.ToString(), team_colour, team_name };
            return atts;
        }

    }

    class CarStats
    {
        public required int brake { get; set; }
        public required DateTime date { get; set; }
        public required int driver_number { get; set; }
        public required int drs { get; set; }
        public required int meeting_key { get; set; }
        public required int n_gear { get; set; }
        public required int rpm { get; set; }
        public required int session_key { get; set; }
        public required int speed { get; set; }
        public required int throttle { get; set; }

        public List<int> getAtts()
        {
            List<int> atts = new List<int>() { brake, n_gear, rpm, speed, throttle, drs };
            return atts;
        }
    }

    public class Meeting
    {
        public required int circuit_key { get; set; }
        public required string circuit_short_name { get; set; }
        public required string country_code { get; set; }
        public required int country_key { get; set; }
        public required string country_name { get; set; }
        public required DateTime date_start { get; set; }
        public required TimeSpan gmt_offset { get; set; }
        public required string location { get; set; }
        public required int meeting_key { get; set; }
        public required string meeting_name { get; set; }
        public required string meeting_official_name { get; set; }
        public required int year { get; set; }
    }
}
