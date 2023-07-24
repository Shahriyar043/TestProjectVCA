using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestProjectVCA
{
    public partial class _Default : Page
    {
        string strCon = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            bool isAuthenticated = Context.User.Identity.IsAuthenticated;
            if (!IsPostBack && isAuthenticated)
            {
                Load();
            }

            if (!isAuthenticated)
            {
                Response.Redirect("~/Account/Login");
            }
        }

        public void Load()
        {
            string userId = Context.User.Identity.GetUserId();

            using (SqlConnection connection = new SqlConnection(strCon))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(string.Format(@"SELECT [Id]
                                              ,[UserId]
                                              ,[FirstName]
                                              ,[LastName]
                                              ,[Status]
                                              ,[CreateDate]
                                              ,[CreateUser]
                                              ,[UpdateDate]
                                              ,[UpdateUser]
                                          FROM [dbo].[Contacts] where Status=1 and UserId='{0}'", userId), connection);
                DataTable dt = new DataTable();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                gridContacs.DataSource = dt;
                gridContacs.DataBind();
                connection.Close();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string userId = Context.User.Identity.GetUserId();
            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;
            string userName = Context.User.Identity.GetUserName();

            using (SqlConnection connection = new SqlConnection(strCon))
            {
                connection.Open();
                string str = string.Format(@"insert into Contacts(UserId,FirstName,LastName,CreateUser) values('{0}','{1}','{2}','{3}')", userId, firstName, lastName, userName);
                SqlCommand cmd = new SqlCommand(str, connection);
                cmd.ExecuteNonQuery();
                lblMessage.Text = "Contact inserted successfully";
                connection.Close();
                Load();
                ClearText();
            }

        }

        protected void lnkSelect_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(strCon))
            {
                LinkButton btn = (LinkButton)sender;
                Session["id"] = btn.CommandArgument;
                connection.Open();
                SqlCommand cmd = new SqlCommand(string.Format(@"SELECT [Id]
                                              ,[UserId]
                                              ,[FirstName]
                                              ,[LastName]
                                              ,[Status]
                                              ,[CreateDate]
                                              ,[CreateUser]
                                              ,[UpdateDate]
                                              ,[UpdateUser]
                                          FROM [dbo].[Contacts] where Id='{0}'", Session["id"]), connection);
                DataTable dt = new DataTable();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                if (dt.Rows.Count >= 0)
                {
                    txtFirstName.Text = dt.Rows[0]["FirstName"].ToString();
                    txtLastName.Text = dt.Rows[0]["LastName"].ToString();

                }
                connection.Close();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;
            string userName = Context.User.Identity.GetUserName();

            using (SqlConnection connection = new SqlConnection(strCon))
            {
                connection.Open();
                string str = string.Format("update Contacts set FirstName='{0}', LastName='{1}',UpdateDate='{2}',UpdateUser='{3}' where Id='{4}'", firstName, lastName, DateTime.Now, userName, Session["id"]);
                SqlCommand cmd = new SqlCommand(str, connection);
                cmd.ExecuteNonQuery();
                lblMessage.Text = "Contact updated successfully";
                connection.Close();
                Load();
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(strCon))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(string.Format("update Contacts set Status=0 where Id='{0}'", Session["id"]), connection);
                cmd.ExecuteNonQuery();
                lblMessage.Text = "Contact deleted";
                connection.Close();
                Load();
            }
        }

        public void ClearText()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
        }
    }
}