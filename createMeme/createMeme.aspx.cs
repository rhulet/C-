using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data;
using MySql.Data.MySqlClient;

public partial class createMeme : System.Web.UI.Page
{
    public string currentUserEmail = null;
    public string currentUserName = null;
    public int currentId = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        string userId = Request["key"];

        string connStr = "server=10.8.1.242;user=ladmin;database=it210b;port=3306;password=chang3m3";
        MySqlConnection conn = new MySqlConnection(connStr);
        if (userId != null)
        {
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "SELECT * FROM cross_server_keys WHERE uuid='" + userId +"'";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                
                while (rdr.Read())
                {
                    currentUserEmail = (string)rdr.GetValue(2);
                }
                rdr.Close();

                if (currentUserEmail != null)
                {
                    sql = "SELECT users.userName, users.userId FROM users WHERE email='" + currentUserEmail + "'";
                    cmd = new MySqlCommand(sql, conn);
                    rdr = cmd.ExecuteReader();
                }

                while (rdr.Read())
                {
                    currentUserName = (string)rdr.GetValue(0);
                    currentId = (int)rdr.GetValue(1);
                }
                rdr.Close();

                userEmailLabel.Text = "Your email: " + currentUserEmail;
                userNameLabel.Text = "Your name: " + currentUserName;
                // Perform database operations
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
        }
        Console.WriteLine("Done.");
        //Guid userId = HttpUtility.ParseQueryString(sender.QueryString);
        System.Diagnostics.Debug.WriteLine(Request["key"]);
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        Session["Path"] = pathBox.Text;
        Session["Alt"] = altBox.Text;
        string connStr = "server=10.8.1.242;user=ladmin;database=it210b;port=3306;password=chang3m3";
        MySqlConnection conn = new MySqlConnection(connStr);
        conn.Open();

        string sql = "INSERT INTO images(imagePath, imageApproved, altText, userId) VALUES('" + Session["Path"] + "', '0', '" + Session["Alt"] + "', '" + currentId + "')";

        MySqlCommand cmd = new MySqlCommand(sql, conn);
        cmd.ExecuteNonQuery();
        conn.Close();

        string node_server_ip = "10.8.1.242";
        WebRequest webRequest = WebRequest.Create("http://" + node_server_ip + ":1337/meme-hook");
        WebResponse webResp = webRequest.GetResponse();

        Response.Redirect("http://" + node_server_ip + ":1337/memes");
    }
}