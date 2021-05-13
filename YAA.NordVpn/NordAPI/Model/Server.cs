using System.Collections.Generic;

namespace NordAPI.Model
{
    public class Category
    {
        public string name { get; set; }
    }

    public class Location
    {
        public double lat { get; set; }
        public double @long { get; set; }
    }

    public class Features
    {
        public bool ikev2 { get; set; }
        public bool openvpn_udp { get; set; }
        public bool openvpn_tcp { get; set; }
        public bool socks { get; set; }
        public bool proxy { get; set; }
        public bool pptp { get; set; }
        public bool l2tp { get; set; }
        public bool openvpn_xor_udp { get; set; }
        public bool openvpn_xor_tcp { get; set; }
        public bool proxy_cybersec { get; set; }
        public bool proxy_ssl { get; set; }
        public bool proxy_ssl_cybersec { get; set; }
        public bool ikev2_v6 { get; set; }
        public bool openvpn_udp_v6 { get; set; }
        public bool openvpn_tcp_v6 { get; set; }
        public bool wireguard_udp { get; set; }
        public bool openvpn_udp_tls_crypt { get; set; }
        public bool openvpn_tcp_tls_crypt { get; set; }
        public bool openvpn_dedicated_udp { get; set; }
        public bool openvpn_dedicated_tcp { get; set; }
        public bool skylark { get; set; }
    }

    public class Server
    {
        public int id { get; set; }
        public string ip_address { get; set; }
        public List<string> search_keywords { get; set; }
        public List<Category> categories { get; set; }
        public string name { get; set; }
        public string domain { get; set; }
        public int price { get; set; }
        public string flag { get; set; }
        public string country { get; set; }
        public Location location { get; set; }
        public int load { get; set; }
        public Features features { get; set; }
    }
}