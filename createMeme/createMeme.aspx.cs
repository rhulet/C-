using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

public partial class createMeme : System.Web.UI.Page
{
    public string currentUserEmail = null;
    public string currentUserName = null;
    public int currentId = -1;
    public string tempFileLocation;

    protected void Page_Load(object sender, EventArgs e)
    {
        Session["previewed"] = false;
        string userId = Request["key"];
        Button1.Enabled = false;

        string connStr = "server=192.168.90.65;user=rhulet;database=it210b;port=3306;password=zxcfrewQ1!";
        MySqlConnection conn = new MySqlConnection(connStr);
        if (userId != null)
        {
            try
            {
                Button1.Enabled = true;
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "SELECT users.userName, users.userId, cross_server_keys.email FROM users INNER JOIN cross_server_keys ON users.email = cross_server_keys.email AND cross_server_keys.uuid = '" + userId +"'";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                
                while (rdr.Read())
                {
                    currentUserEmail = (string)rdr.GetValue(2);
                    Session["email"] = currentUserEmail;
                    currentUserName = (string)rdr.GetValue(0);
                    Session["user"] = currentUserName;
                    currentId = (int)rdr.GetValue(1);
                    Session["userId"] = currentId;
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
        if (Button1.Text == "Upload")
        {
            if (FileUpload1.HasFile)
            {
                string tempFileName = FileUpload1.PostedFile.FileName;
                string tempGuid = Guid.NewGuid().ToString();
                Session["tempFileLocation"] = tempGuid + tempFileName;
                FileUpload1.PostedFile.SaveAs(Server.MapPath("~/tempImages/") + tempGuid + tempFileName);
                Button1.Text = "Create Meme";
                pathLabel.Visible = true;
                pathBox.Visible = true;
                altBox.Visible = true;
                altLabel.Visible = true;
                Preview.Visible = true;
                filterSel.Visible = true;
                filterSelLabel.Visible = true;
                textAlign.Visible = true;
                textAlignLabel.Visible = true;
            }
        }
        else if (Button1.Text == "Create Meme")
        {
            if ((bool)Session["previewed"] == false)
            {
                previewClick(sender, e);
            }

            Session["Alt"] = altBox.Text;
            string connStr = "server=192.168.90.65;user=rhulet;database=it210b;port=3306;password=zxcfrewQ1!";
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();

            string sqlImagePath = @"\Images\" + (string)Session["tempFileLocation"];
            string sql = "INSERT INTO images(imagePath, imageApproved, altText, userId) VALUES('" + @"/Images/" + (string)Session["tempFileLocation"] + "', '0', '" + Session["Alt"] + "', '" + currentId + "')";

            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            conn.Close();

            Bitmap bmp = new Bitmap(Server.MapPath("~/tempImages/") + (string)Session["editedImg"]);
            bmp.Save(Server.MapPath("~/Images/") + (string)Session["tempFileLocation"]);
            bmp.Dispose();

            DirectoryInfo di = new DirectoryInfo(Server.MapPath("~/tempImages/"));

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }

            string node_server_ip = "nodejs.rhulet34.it210.it.et.byu.edu";
            WebRequest webRequest = WebRequest.Create("http://" + node_server_ip + "/meme-hook");
            WebResponse webResp = webRequest.GetResponse();

            Response.Redirect("http://" + node_server_ip + "/memes");
        }
    }

    protected void previewClick(object sender, EventArgs e)
    {
        Session["previewed"] = true;
        previewBox.Visible = false;
        applySettings((string)Session["tempFileLocation"], filterSel.SelectedValue, pathBox.Text);
        previewBox.ImageUrl = Server.MapPath("~/tempImages/") + (string)Session["editedImg"];
        previewBox.Visible = true;
    }

    public void applySettings(string path, string filter, string text)
    {
        Bitmap bmp = new Bitmap(Server.MapPath("~/tempImages/") + path);

        if (filter != "0")
        {
            bmp = applyFilter(bmp, filter);
        }

        if (text != "")
        {
            bmp = writeText(bmp, text);
        }

        Session["editedImg"] = "edited" + path;
        saveImage(bmp, (string)Session["editedImg"]);
        bmp.Dispose();
    }

    public Bitmap applyFilter(Bitmap bmp, string filter)
    {
        //read image
        

        //load original image in picturebox1
        //pictureBox1.Image = Image.FromFile(path);

        //get image dimension
        int width = bmp.Width;
        int height = bmp.Height;

        //color of pixel
        Color p;

        switch (filter)
        {
            case "1":
                //sepia
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        //get pixel value
                        p = bmp.GetPixel(x, y);

                        //extract pixel component ARGB
                        int a = p.A;
                        int r = p.R;
                        int g = p.G;
                        int b = p.B;

                        //calculate temp value
                        int tr = (int)(0.393 * r + 0.769 * g + 0.189 * b);
                        int tg = (int)(0.349 * r + 0.686 * g + 0.168 * b);
                        int tb = (int)(0.272 * r + 0.534 * g + 0.131 * b);

                        //set new RGB value
                        if (tr > 255)
                        {
                            r = 255;
                        }
                        else
                        {
                            r = tr;
                        }

                        if (tg > 255)
                        {
                            g = 255;
                        }
                        else
                        {
                            g = tg;
                        }

                        if (tb > 255)
                        {
                            b = 255;
                        }
                        else
                        {
                            b = tb;
                        }

                        //set the new RGB value in image pixel
                        bmp.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                    }
                };
                break;
            case "2":
                //get a graphics object from the new image
                Graphics gra = Graphics.FromImage(bmp);

                //create the grayscale ColorMatrix
                ColorMatrix colorMatrix = new ColorMatrix(
                   new float[][]
                   {
                        new float[] {.3f, .3f, .3f, 0, 0},
                        new float[] {.59f, .59f, .59f, 0, 0},
                        new float[] {.11f, .11f, .11f, 0, 0},
                        new float[] {0, 0, 0, 1, 0},
                        new float[] {0, 0, 0, 0, 1}
                   });

                //create some image attributes
                ImageAttributes attributes = new ImageAttributes();

                //set the color matrix attribute
                attributes.SetColorMatrix(colorMatrix);

                //draw the original image on the new image
                //using the grayscale color matrix
                gra.DrawImage(bmp, new Rectangle(0, 0, width, height),
                   0, 0, width, height, GraphicsUnit.Pixel, attributes);

                //dispose the Graphics object
                gra.Dispose();
                break;
            default:
                break;
        }
        return bmp;
    }

    public Bitmap writeText(Bitmap bitMapImage, string text)
    {
        //Load the Image to be written on.
        Graphics graphicImage = Graphics.FromImage(bitMapImage);

        //Smooth graphics is nice.
        graphicImage.SmoothingMode = SmoothingMode.AntiAlias;

        //this will center align our text at the bottom of the image
        StringFormat sf = new StringFormat();
        sf.Alignment = StringAlignment.Center;

        if (textAlign.SelectedValue == "0")
        {
            sf.LineAlignment = StringAlignment.Near;
        }
        else if (textAlign.SelectedValue == "1")
        {
            sf.LineAlignment = StringAlignment.Far;
        }
        

        //pen for outline - set width parameter
        Pen p = new Pen(ColorTranslator.FromHtml("#000000"), 16);
        p.LineJoin = LineJoin.Round; //prevent "spikes" at the path

        //this will be the rectangle used to draw and auto-wrap the text.
        //basically = image size
        Rectangle r = new Rectangle(0, 0, bitMapImage.Width, bitMapImage.Height);
        Font f = new Font("Impact", 120, FontStyle.Bold, GraphicsUnit.Pixel);

        GraphicsPath gp = new GraphicsPath();
        gp.AddString(text,
             f.FontFamily, (int)f.Style, 120, r, sf);

        //Write your text.
        graphicImage.DrawPath(p, gp);
        graphicImage.FillPath(Brushes.White, gp);

        gp.Dispose();
        f.Dispose();
        sf.Dispose();
        graphicImage.Dispose();

        return bitMapImage;
    }

    private void saveImage(Bitmap image, string name)
    {
        if (File.Exists(Server.MapPath("~/tempImages/") + name))
            File.Delete(Server.MapPath("~/tempImages/") + name);

        image.Save(Server.MapPath("~/tempImages/") + name);
    }
}