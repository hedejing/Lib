using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;

namespace Lib
{
    public partial class Form1 : Form
    {
        //global variable
        SqlConnection connection;
        bool IsConnect = false;

        public Form1()
        {
            InitializeComponent();
            InitDataGrid();
            tabControl1.TabPages.Remove(tabPage3);
            tabControl1.TabPages.Remove(tabPage4);
            tabControl1.TabPages.Remove(tabPage5);
        }

        private void InitDataGrid()
        {
            string connectString = null;
            SqlConnection connection;
            string sql = "select * from book where bno = 0";
            connectString = "Data Source = (localdb)\\ProjectsV12;Initial Catalog=Library;Integrated Security=SSPI;";
            connection = new SqlConnection(connectString);
            try
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                DataSet dataset = new DataSet();
                adapter.Fill(dataset);

                dataGridView1.Visible = true;
                dataGridView1.DataSource = dataset.Tables[0];
                dataGridView2.Visible = true;
                dataGridView2.DataSource = dataset.Tables[0];
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void test()
        {
            string connectString = null;
            SqlConnection connection;
            SqlCommand command;
            string sql;
            SqlDataReader dataReader;
            connectString = "Data Source = (localdb)\\ProjectsV12;Initial Catalog=Library;Integrated Security=SSPI;";
            sql = "select * from book;select *from card";
            connection = new SqlConnection(connectString);
            try
            {
                connection.Open();
                command = new SqlCommand(sql, connection);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    MessageBox.Show(dataReader.GetValue(0) + "-" + dataReader.GetValue(1) + "-" + dataReader.GetValue(2));
                }
                dataReader.Close();
                command.Dispose();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string connectString = null;
            SqlConnection connection;
            connectString = "Data Source = (localdb)\\ProjectsV12;Initial Catalog=Library;Integrated Security=SSPI;";
            connection = new SqlConnection(connectString);
            SqlCommand command = new SqlCommand(null,connection);
            command.CommandText = "select * from book where ";
            if(BnoQuery.TextLength>0)
            {
                command.CommandText += " bno = @_bno and";
                command.Parameters.Add("@_bno", SqlDbType.Char, 8);
                command.Parameters["@_bno"].Value = BnoQuery.Text;
            }
            if(CategoryQuery.TextLength>0)
            {
                command.CommandText += " category = @_category and ";
                command.Parameters.Add("@_category", SqlDbType.Char, 10);
                command.Parameters["@_category"].Value = CategoryQuery.Text;
            }
            if(TitleQuery.TextLength>0)
            {
                command.CommandText += " title = @_title and ";
                command.Parameters.Add("@_title", SqlDbType.VarChar, 40);
                command.Parameters["@_title"].Value = TitleQuery.Text;
            }
            if(PressQuery.TextLength>0)
            {
                command.CommandText += " press = @_press and ";
                command.Parameters.Add("@_press", SqlDbType.VarChar, 30);
                command.Parameters["@_press"].Value = PressQuery.Text;
            }
            command.CommandText += " year <= @_year_max and year >= @_year_min and ";
            command.Parameters.Add("@_year_max", SqlDbType.Int, 0);
            command.Parameters.Add("@_year_min", SqlDbType.Int, 0);
            command.Parameters["@_year_min"].Value = YearMin.Value;
            command.Parameters["@_year_max"].Value = YearMax.Value;
            if(AuthorQuery.TextLength>0)
            {
                command.CommandText += " author = @_author and ";
                command.Parameters.Add("@_author", SqlDbType.VarChar, 20);
                command.Parameters["@_author"].Value = AuthorQuery.Text;
            }
            if(PriceMax.Value>0)
            {
                command.CommandText += " price <= @_price_max and ";
                command.Parameters.Add("@_price_max", SqlDbType.Decimal, 4);
                command.Parameters["@_price_max"].Value = PriceMax.Value;
                command.Parameters["@_price_max"].Precision = 4;
                command.Parameters["@_price_max"].Scale = 2;
            }
            command.CommandText += " price >= @_price_min and  ";
            command.Parameters.Add("@_price_min", SqlDbType.Int, 0);
            command.Parameters["@_price_min"].Value = PriceMin.Value;
            command.Parameters["@_price_min"].Precision = 4;
            command.Parameters["@_price_min"].Scale = 2;
            command.CommandText += " stock >= @_stock ";
            command.Parameters.Add("@_stock", SqlDbType.Int, 0);
            command.Parameters["@_stock"].Value = StockMin.Value;
            try
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(null, connection);
                adapter.SelectCommand = command;
                DataSet dataset = new DataSet();
                adapter.Fill(dataset);

                dataGridView1.Visible = true;
                dataGridView1.DataSource = dataset.Tables[0];
                
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
           YearQuery.Visible = Yearlabel.Visible = true;
           PressQuery.Visible = Presslabel.Visible = true;
           StockQuery.Visible = Stocklabel.Visible = true;
           PriceQuery.Visible = Pricelabel.Visible = true;
           label18.Visible = label19.Visible = label20.Visible = true;
           label29.Visible = label30.Visible = true;
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            //SqlConnection connection;
            string connectString = null;
            if (UserText.TextLength == 0 && PasswordText.TextLength == 0)
                return;
            string UserName = UserText.Text;
            string Password = PasswordText.Text;

            connectString = "Data Source = (localdb)\\ProjectsV12;Initial Catalog = Library;User ID = " + UserName + " ;Password = " + Password;
            connection = new SqlConnection(connectString);
            try
            {
                connection.Open();
                MessageBox.Show("Login Succeed.");
                IsConnect = true;
                tabControl1.TabPages.Add(tabPage3);
                tabControl1.TabPages.Add(tabPage4);
                tabControl1.TabPages.Add(tabPage5);
                UserText.Clear();
                PasswordText.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Logoutbutton_Click(object sender, EventArgs e)
        {
            try
            {
                if(connection.State==ConnectionState.Closed)
                {
                    MessageBox.Show("You haven't log in yet.");
                    return;
                }
                connection.Close();
                IsConnect = false;
                tabControl1.TabPages.Remove(tabPage3);
                tabControl1.TabPages.Remove(tabPage4);
                tabControl1.TabPages.Remove(tabPage5);
                MessageBox.Show("Logout Succeed.");
            }
            catch
            {
                MessageBox.Show("You haven't log in yet.");
            }
        }

        private void Clearallbutton_Click(object sender, EventArgs e)
        {
            BnoQuery.Clear();
            TitleQuery.Clear();
            AuthorQuery.Clear();
            PressQuery.Clear();
            CategoryQuery.Clear();
        }

        private void Borrowbutton_Click(object sender, EventArgs e)
        {
            Int32 stock;
            string target;
            SqlCommand command = null;
            SqlDataReader dataReader = null;
            if (BRbookno.TextLength == 0 || BRcardno.TextLength == 0)
                return;
            try
            {
                command = new SqlCommand(null, connection);
                command.CommandText = "select stock from book where bno = @_bno";
                SqlParameter bnoParameter = new SqlParameter("@_bno",SqlDbType.Char,8);
                bnoParameter.Value = BRbookno.Text;
                command.Parameters.Add(bnoParameter);
                command.Prepare();
                dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    target = dataReader.GetValue(0).ToString();
                    stock = Convert.ToInt32(target);
                    if(stock<=0)
                    {
                        dataReader.Dispose();
                        command.Dispose();
                        command = new SqlCommand(null, connection);
                        command.CommandText = "select min(return_date) from borrow where bno = @_bno";
                        command.Parameters.Add("@_bno", SqlDbType.Char, 8);
                        command.Parameters["@_bno"].Value = BRbookno.Text;
                        command.Prepare();
                        dataReader = command.ExecuteReader();
                        if(dataReader.Read())
                        {
                            target = dataReader.GetValue(0).ToString();
                            string text = "None of them is in library right now, the closest return date is " + target + ". ";
                            MessageBox.Show(text);
                        }
                        dataReader.Dispose();
                        command.Dispose();
                        return;   
                    }
                    dataReader.Dispose();
                    command.Dispose();
                }
                else
                {
                    MessageBox.Show("There is no such book in the library.");
                    dataReader.Dispose();
                    command.Dispose();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show( command.CommandText + ":" + ex.Message);
            }
            try
            {
                command = new SqlCommand(null, connection);
                DateTime _return_date = DateTime.Now.AddMonths(1);
                command.CommandText = "insert into borrow values(@_cardno,@_bookno,'" 
                    +DateTime.Now.ToString()+ "','" + _return_date.ToString() +"')";
                SqlParameter _cardno = new SqlParameter("@_cardno",SqlDbType.Char,7);
                _cardno.Value = BRcardno.Text;
                SqlParameter _bookno = new SqlParameter("@_bookno",SqlDbType.Char,8);
                _bookno.Value = BRbookno.Text;
                command.Parameters.Add(_cardno);
                command.Parameters.Add(_bookno);
                command.Prepare();
                command.ExecuteNonQuery();
                MessageBox.Show("Borrowing Succeed.");
                command.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(command.CommandText + ":" + ex.Message);
            }
            try
            {
                command = new SqlCommand(null, connection);
                command.CommandText = "select stock from book where bno = @_bookno";
                SqlParameter _bookno = new SqlParameter("@_bookno", SqlDbType.Char, 8);
                _bookno.Value = BRbookno.Text;
                command.Parameters.Add(_bookno);
                command.Prepare();
                dataReader = command.ExecuteReader();
                if(dataReader.Read())
                {
                    target = dataReader.GetValue(0).ToString();
                    dataReader.Dispose();
                    command.Dispose();
                    stock = Convert.ToInt32(target);
                    stock = stock - 1;
                    target = Convert.ToString(stock);
                    command = new SqlCommand(null, connection);
                    command.CommandText = "update book set stock = " + target + " where bno = @_bookno";
                    command.Parameters.Add("@_bookno", SqlDbType.Char, 8);
                    command.Parameters["@_bookno"].Value = BRbookno.Text;
                    command.Prepare();
                    command.ExecuteNonQuery();
                    command.Dispose();
                }    
            }
            catch (Exception ex)
            {
                MessageBox.Show(command.CommandText + ":" + ex.Message);
            }
            DataGrid2Refresh();

        }

        private void Returnbutton_Click(object sender, EventArgs e)
        {
            SqlCommand command = null ;
            Int32 stock;
            string target;
            if (BRbookno.TextLength == 0 && BRcardno.TextLength == 0)
                return;
            try
            {
                command = new SqlCommand(null, connection);
                command.CommandText = "delete from borrow where borrow_date >= all(select borrow_date from borrow where cno = @_cno and bno = @_bno) ";
                command.Parameters.Add("@_bno", SqlDbType.Char, 8);
                command.Parameters.Add("@_cno", SqlDbType.Char, 7);
                command.Parameters["@_bno"].Value = BRbookno.Text;
                command.Parameters["@_cno"].Value = BRcardno.Text;
                command.Prepare();
                if (command.ExecuteNonQuery() > 0)
                    MessageBox.Show("Returning Succeed.");
                else
                    MessageBox.Show("There is no such book or such card in the library. ");
                command.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(command.CommandText + ":" + ex.Message);
            }
            try
            {
                command = new SqlCommand(null, connection);
                command.CommandText = "select stock from book where bno = @_bno";
                command.Parameters.Add("@_bno", SqlDbType.Char, 8);
                command.Parameters["@_bno"].Value = BRbookno.Text;
                command.Prepare();
                SqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    target = dataReader.GetValue(0).ToString();
                    command.Dispose();
                    stock = Convert.ToInt32(target);
                    stock = stock + 1;
                    target = Convert.ToString(stock);
                    command = new SqlCommand(null, connection);
                    command.CommandText = "update book set stock = " + target + " where bno = @_bno";
                    command.Parameters.Add("@_bno", SqlDbType.Char, 8);
                    command.Parameters["@_bno"].Value = BRbookno.Text;
                    command.Prepare();
                    command.ExecuteNonQuery();
                }
                dataReader.Dispose();
                command.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(command.CommandText + ":" + ex.Message);
            }
            DataGrid2Refresh();
        }

        private void Newbutton_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand(null, connection);
            command.CommandText = "insert into card values(@_cno,@_name,@_department,@_type)";
            command.Parameters.Add("@_cno", SqlDbType.Char, 7);
            command.Parameters.Add("@_name", SqlDbType.VarChar, 10);
            command.Parameters.Add("@_department", SqlDbType.VarChar, 40);
            command.Parameters.Add("@_type", SqlDbType.Char, 1);
            command.Parameters["@_cno"].Value = CCardno.Text;
            command.Parameters["@_name"].Value = CUsername.Text;
            command.Parameters["@_department"].Value = CDepartment.Text;
            command.Parameters["@_type"].Value = Typebox.Text[0];
            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("A new card is created. ");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            command.Dispose();
        }

        private void Deletebutton_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand(null, connection);
            command.CommandText = "delete from card where cno = @_cno and cname = @_name and department = @_department and type = @_type";
            command.Parameters.Add("@_cno", SqlDbType.Char, 7);
            command.Parameters.Add("@_name", SqlDbType.VarChar, 10);
            command.Parameters.Add("@_department", SqlDbType.VarChar, 40);
            command.Parameters.Add("@_type", SqlDbType.Char, 1);
            command.Parameters["@_cno"].Value = CCardno.Text;
            command.Parameters["@_name"].Value = CUsername.Text;
            command.Parameters["@_department"].Value = CDepartment.Text;
            command.Parameters["@_type"].Value = Typebox.Text[0];
            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("This card is deleted. ");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            command.Dispose();
        }

        private void BRcardno_Leave(object sender, EventArgs e)
        {
            DataGrid2Refresh();
        }

        private void DataGrid2Refresh()
        {
            if (BRcardno.TextLength == 0)
                return;
            string sql = "select * from book where book.bno in(select borrow.bno from borrow where cno = '" + BRcardno.Text + "')";
            try
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, connection);
                DataSet dataset = new DataSet();
                dataAdapter.Fill(dataset);
                dataGridView2.DataSource = dataset.Tables[0];
                dataAdapter.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            test();
        }

        private void Storebutton_Click(object sender, EventArgs e)
        {
            if(Sbookno.TextLength==0||Scategory.TextLength==0||Stitle.TextLength==0||Spress.TextLength==0||Syear.TextLength==0||Sauthor.TextLength==0||Sprice.TextLength==0||Snumber.TextLength==0)
            {
                MessageBox.Show("Some information is lack.");
                return;
            }
            try
            {
                SqlCommand command = new SqlCommand(null, connection);
                command.CommandText = "insert into book values(@_bno,@_category,@_title,@_press,@_year,@_author,@_price,@_total,@_stock)";
                command.Parameters.Add("@_bno", SqlDbType.Char, 8);
                command.Parameters.Add("@_category", SqlDbType.Char, 10);
                command.Parameters.Add("@_title", SqlDbType.VarChar, 40);
                command.Parameters.Add("@_press", SqlDbType.VarChar, 30);
                command.Parameters.Add("@_year", SqlDbType.Int, 0);
                command.Parameters.Add("@_author", SqlDbType.VarChar, 20);
                command.Parameters.Add("@_price", SqlDbType.Decimal, 4);
                command.Parameters.Add("@_total", SqlDbType.Int, 0);
                command.Parameters.Add("@_stock", SqlDbType.Int, 0);
                command.Parameters["@_price"].Precision = 4;
                command.Parameters["@_price"].Scale = 2;

                command.Parameters["@_bno"].Value = Sbookno.Text;
                command.Parameters["@_category"].Value = Scategory.Text;
                command.Parameters["@_title"].Value = Stitle.Text;
                command.Parameters["@_press"].Value = Spress.Text;
                command.Parameters["@_year"].Value = Syear.Text;
                command.Parameters["@_author"].Value = Sauthor.Text;
                command.Parameters["@_price"].Value = Sprice.Text;
                command.Parameters["@_total"].Value = command.Parameters["@_stock"].Value = Snumber.Text;
                command.Prepare();
                command.ExecuteNonQuery();
                MessageBox.Show("Store Succeed.");
                command.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Stocebyfilebutton_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Templates);
                if(openFileDialog1.ShowDialog()==DialogResult.OK)
                {
                    Stream stream = openFileDialog1.OpenFile();
                    StreamReader reader = new StreamReader(stream);
                    string line;
                    SqlCommand command = new SqlCommand(null,connection);
                    command.CommandText = "insert into book values(@_bno,@_category,@_title,@_press,@_year,@_author,@_price,@_total,@_stock)";
                    command.Parameters.Add("@_bno", SqlDbType.Char, 8);
                    command.Parameters.Add("@_category", SqlDbType.Char, 10);
                    command.Parameters.Add("@_title", SqlDbType.VarChar, 40);
                    command.Parameters.Add("@_press", SqlDbType.VarChar, 30);
                    command.Parameters.Add("@_year", SqlDbType.Int, 0);
                    command.Parameters.Add("@_author", SqlDbType.VarChar, 20);
                    command.Parameters.Add("@_price", SqlDbType.Decimal, 4);
                    command.Parameters.Add("@_total", SqlDbType.Int, 0);
                    command.Parameters.Add("@_stock", SqlDbType.Int, 0);
                    command.Parameters["@_price"].Precision = 4;
                    command.Parameters["@_price"].Scale = 2;
                    char[] delimiterChars = {','};
                    while((line = reader.ReadLine())!=null)
                    {
                        string[] words = line.Split(delimiterChars);
                        command.Parameters["@_bno"].Value = words[0];
                        command.Parameters["@_category"].Value = words[1];
                        command.Parameters["@_title"].Value = words[2];
                        command.Parameters["@_press"].Value = words[3];
                        command.Parameters["@_year"].Value = words[4];
                        command.Parameters["@_author"].Value = words[5];
                        command.Parameters["@_price"].Value = words[6];
                        command.Parameters["@_total"].Value = command.Parameters["@_stock"].Value = words[7];
                        command.Prepare();
                        command.ExecuteNonQuery();
                    }
                    command.Dispose();
                    MessageBox.Show("Store by file succeed. ");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
           
       }
}
