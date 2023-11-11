using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using PasswdMan;

namespace WPM
{


    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public string loginuser = "";   //Global name of user currently logged in
        public string entrytype;       
        public int passwdlimit = 8;
        public int usrnamminlength = 6;
        Application newentry = new Application();

        public string encript(string pw)
        {
            string returnpw = "";
            byte[] asciiCodes = Encoding.ASCII.GetBytes(pw.Trim());             // Create array for acsii values 
            for (int i = 0; i < pw.Length; i++)                                 // Loop the number of times of the length of the password
            {
                int coded = asciiCodes[i] + i + 1;                               // Using an incrementaial Casear Ciepaer 
                if (coded > 126)                                                   
                    coded = coded - 94;                                             // Take the acsii value of the charther add or subtract the loop variabule to it   
                char letter = Convert.ToChar(coded);                        
                returnpw = returnpw + letter;                                        // covert it back to an ascii chather and add it to the return vaule 
            }

            decript(returnpw);
            return returnpw;

        }

        private string decript(string pw)                                   // Using an incrementaial Casear Ciepaer 
        {
            string returnpw = "";
            byte[] asciiCodes = Encoding.ASCII.GetBytes(pw.Trim());         // Take the acsii value of the charther add or subtract the loop variabule to it 
            for (int i = 0; i < pw.Length; i++)
            {
                int coded = asciiCodes[i] - i - 1;                           // covert it back to an ascii chather and add it to the return vaule 
                if (coded < 33)
                    coded = coded + 94;

                char letter = Convert.ToChar(coded);
                returnpw = returnpw + letter;
            }
            return returnpw;
        }

        private string randompasswd()                           // randomly generate a interger value in the range of 33 to 126 
        {                                                       // convert that interger into a asscii chather and add it to the return value 
            string returnpw = "";                               // Repeat this for the predetremented passsword length 
            Random passwdchar = new Random();
            for (int i = 1;i <= passwdlimit;i++)
            {
                int asciichar = passwdchar.Next(33, 126);
                char letter = Convert.ToChar(asciichar);
                returnpw = returnpw + letter;
            }

            return returnpw;
        }



        private class User                                              // Create user class 
        {
            private string Name, Account, Username, Password;           // Create standard setters and getters 
            public void setName(string name) { Name = name; }
            public string getName() { return Name; }                                
            public void setUsername(string username) { Username = username; }
            public string getUsername() { return Username; }
            public void setPassword(string password) { Password = password; }
            public string getPassword() { return Password; }
            public void setAccount(string account) { Account = account; }
            public string getAccount() { return Account; }

            public void addUseraccount()                                        // Create sql connection to database 
            {                                                                   // Assign all user attributes to placeholders 
                var connection = new SQLiteConnection();
                connection.ConnectionString = "Data Source=passman.db";         

                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    SQLiteCommand sqlite_insert;
                    sqlite_insert = connection.CreateCommand();                     // execute query to instread values 
                    sqlite_insert.CommandText = "INSERT INTO users (username,userpasswd,acctype) VALUES (@usrnam,@usrpw,@usracc)";
                    sqlite_insert.Parameters.AddWithValue("@usrnam", this.Username);
                    sqlite_insert.Parameters.AddWithValue("@usrpw",  this.Password);
                    sqlite_insert.Parameters.AddWithValue("@usracc", this.Account );
                    sqlite_insert.ExecuteNonQuery();
                }
                connection.Close();
            }
        }



        class Application               // Create application class 
        {
            public Application() { }
            private string Name,Username,Entryuser,Entrypass,Detail,Created,Updated,Type;
            //public string Type = "application";
            public void setName(string name) {Name = name;}
            public string getType() {return Type;}                      // All stanard setters and getters 
            public void setType(string type) { Type = type; }
            public string getName() { return Name; }
            public void setUsername(string username) { Username = username; }
            public string getUsername() { return Username; }
            public void setCreated(string created) { Created = created; }
            public string getCreated() { return Created; }
            public void setUpdated(string updated) { Updated = updated; }
            public string getUpdated() { return Updated; }
            public void setEntryuser(string entryuser) { Entryuser = entryuser; }
            public string getEntryuser() { return Entryuser; }
            public void setEntrypass(string entrypass) {Entrypass = entrypass ; }
            public string getEntrypass() { return Entrypass; }
            public void setDetail(string detail) { Detail = detail; }
            public string getDetail() { return Detail; }

            public void saveEntry()                                     // Saves a entry assign all entry attirubutes to placeholders excute query to create new recordd
            { 
                var connection = new SQLiteConnection();
                connection.ConnectionString = "Data Source=passman.db";

                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    SQLiteCommand sqlite_insert;
                    sqlite_insert = connection.CreateCommand();
                    sqlite_insert.CommandText = "INSERT INTO entries (username,entryname,entryusername,entrypasswd, entrytype, extradetail,created,modified) VALUES (@usrnam,@enam,@eunam,@epasswd,@etyp,@edet,@cr,@up)";
                    sqlite_insert.Parameters.AddWithValue("@usrnam", this.Username);
                    sqlite_insert.Parameters.AddWithValue("@enam", this.Name);
                    sqlite_insert.Parameters.AddWithValue("@eunam", this.Entryuser);
                    sqlite_insert.Parameters.AddWithValue("@epasswd", this.Entrypass);
                    sqlite_insert.Parameters.AddWithValue("@etyp", this.Type);
                    sqlite_insert.Parameters.AddWithValue("@edet", this.Detail);
                    sqlite_insert.Parameters.AddWithValue("@cr", this.Created);
                    sqlite_insert.Parameters.AddWithValue("@up", this.Updated);
                    sqlite_insert.ExecuteNonQuery();
                }
                connection.Close();
            }

            public void updateEntry()
            {
                var connection = new SQLiteConnection();
                connection.ConnectionString = "Data Source=passman.db";
                                                                            // Saves a entry assign all entry attirubutes to placeholders excute query to update new recordd
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    SQLiteCommand sqlite_insert;
                    sqlite_insert = connection.CreateCommand();
                    sqlite_insert.CommandText = ("UPDATE entries SET entryusername = @eunam , entrypasswd = @epasswd , extradetail = @edet ,  modified = @up  WHERE username = @un AND entryname = @enam");
                    sqlite_insert.Parameters.AddWithValue("@enam", this.Name);
                    sqlite_insert.Parameters.AddWithValue("@eunam", this.Entryuser);
                    sqlite_insert.Parameters.AddWithValue("@epasswd", this.Entrypass);
                    sqlite_insert.Parameters.AddWithValue("@edet", this.Detail);            
                    sqlite_insert.Parameters.AddWithValue("@up", this.Updated);
                    sqlite_insert.Parameters.AddWithValue("@un", this.Username);
                    sqlite_insert.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        class Website : Application
        {
            public string type = "website";                                 // create website class which inherits application class 
        }

        class Game : Application
        {
            public string type = "game";                                  // create game class which inherits application class 
        }



        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();                            // exits the program
        }

        private void entrytypecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (entrytypecomboBox.Text != "Application") //only enable extra text input if not an application
                detailtextBox.Enabled = true;
            else
                detailtextBox.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            initalise_db();
          

            if(usernameExists(usernametextBox.Text))
                loginuser = usernametextBox.Text;
          

            if (userLogin(loginuser, encript(loginpasswordBox.Text)) == "admin") //Check user type and enable features acordingly
                admingroupBox.Enabled = true; 
                adminToolStripMenuItem.Enabled = true;


          // showusers(usernametextBox.Text );
        //showentries(loginuser);
        }

        private void initalise_db()                     // instliase the database 
        {
            var connection = new SQLiteConnection();
            connection.ConnectionString = "Data Source=passman.db";

            connection.Open();
            if (connection.State == ConnectionState.Open)
            {
                SQLiteCommand sqlite_cmd;
                string Create_users = "CREATE TABLE IF NOT EXISTS users(username TEXT(20) PRIMARY KEY,userpasswd TEXT(20), acctype TEXT(10))";
                string Create_entries = "CREATE TABLE IF NOT EXISTS entries(username TEXT(20), entryname TEXT(20),entryusername TEXT(20), entrypasswd TEXT(20), entrytype TEXT(10),extradetail text(40))";
                sqlite_cmd = connection.CreateCommand();
                sqlite_cmd.CommandText = Create_users;
                sqlite_cmd.ExecuteNonQuery();
                sqlite_cmd.CommandText = Create_entries;
                sqlite_cmd.ExecuteNonQuery();
            }
            connection.Close();
        }

        private void deleteUser(string user)
        {
            var connection = new SQLiteConnection();
            connection.ConnectionString = "Data Source=passman.db";

            connection.Open();                                       // Saves a entry assign all entry attirubutes to placeholders excute query to delete new recordd
            if (connection.State == ConnectionState.Open)
            {
                SQLiteCommand sqlite_delete;
                sqlite_delete = connection.CreateCommand();
                sqlite_delete.CommandText = "DELETE FROM users WHERE username = @usr";
                sqlite_delete.Parameters.AddWithValue("@usr", user);
                sqlite_delete.ExecuteNonQuery();
            }
            connection.Close();
        }

        private void deleteEntry(string entry, string user)
        {
            var connection = new SQLiteConnection();
            connection.ConnectionString = "Data Source=passman.db";

            connection.Open();
            if (connection.State == ConnectionState.Open)
            {
                SQLiteCommand sqlite_delete;
                sqlite_delete = connection.CreateCommand();
                sqlite_delete.CommandText = "DELETE FROM entries WHERE username = @un AND entryname = @ent ";
                sqlite_delete.Parameters.AddWithValue("@un", user);
                sqlite_delete.Parameters.AddWithValue("@ent", entry);
                sqlite_delete.ExecuteNonQuery();
            }
            connection.Close();
        }


        public string userLogin(string user,string password)            //function to check if the user exists and are they admin
        {
            string account = "";
            var connection = new SQLiteConnection();                    //Create ne SQLite connection
            connection.ConnectionString = "Data Source=passman.db";     //Assigng a database to the connection

            connection.Open();                                          //Open the connection

            SQLiteCommand sqlite_login;                                 //create a SQL command instance
            sqlite_login = connection.CreateCommand();                  //create a connection for that instance
            sqlite_login.CommandText = @"select  acctype FROM users WHERE username = @usr AND userpasswd = @usrpw"; //Create SLQ query string
            sqlite_login.Parameters.AddWithValue("@usr", user);         //assign an incomming value to a placeholder
            sqlite_login.Parameters.AddWithValue("@usrpw", password);   //assign an incomming value to a placeholder

            using (var reader = sqlite_login.ExecuteReader())           //run reader command to get data from table  

                if (reader.Read())                                      //was there anythin to read
                    account = $"{reader["acctype"]}";                   //if so, assign the account type (priveleges) to a variable
                else
                    MessageBox.Show("Incorrect Password");              //if not give warning
                
            connection.Close();                                         //don't leave the connection open
            return account;                                             //return variable to tell if user is admin or not
        }


        public bool usernameExists(string user)                                                 //check if user exists
        {
            bool found = false;                                                                 //set found boolean
            var connection = new SQLiteConnection();                                            //create new connection instance
            connection.ConnectionString = "Data Source=passman.db";                             //state which database we are ising
            usergroupBox.Visible = true;
            connection.Open();                                                                  //open the connection

            SQLiteCommand sqlite_exists;                                                        //create a new query command
            sqlite_exists = connection.CreateCommand();                                         //create a new query command
            sqlite_exists.CommandText = @"select  username FROM users WHERE username = @usr";   //state the command query to run
            sqlite_exists.Parameters.AddWithValue("@usr", user);                                //assign incommining value to placeholder

            using (var reader = sqlite_exists.ExecuteReader())                                  //call reader to execute query

            if (reader.Read())                                                                  //if user has been found
                found = true;                                                                   //toggle found boolean
            else
                MessageBox.Show("User Not Found");                                              //if not found then report

            connection.Close();                                                                 //close connection
            return found;                                                                       //return found or not

        }


        private void showusers()
        {
            var connection = new SQLiteConnection();                                    //create new connection instance
            connection.ConnectionString = "Data Source=passman.db";                     //state which database we are ising
            connection.Open();                                                          //open the connection
            SQLiteCommand comm = new SQLiteCommand("Select * from users ", connection); //create a new query command
            userdataGridView.Rows.Clear();                                              //clear the display grid
            userdataGridView.Refresh();                                                 //refresh the display grid
            using (SQLiteDataReader read = comm.ExecuteReader())                        //run reader to execute and read the resuluts
            {
                while (read.Read())                                                     //start loop until all is read
                {
                userdataGridView.Rows.Add(new object[] {                                //add new row to table for each read
                read.GetValue(read.GetOrdinal("username")),                             //read table values and display in column
                read.GetValue(read.GetOrdinal("userpasswd")),                           //read table values and display in column
                read.GetValue(read.GetOrdinal("acctype"))                               //read table values and display in column
            });
                }
            }
        }

        private void applicationradioButton_CheckedChanged(object sender, EventArgs e)
        {
          /*  Application newentry = new Application();
            MessageBox.Show(newentry.type);
            newentry.setName(entrynametextBox.Text);
            newentry.setEntryuser(entrynametextBox.Text);
            newentry.setEntrypass(entrynametextBox.Text);
            newentry.setCreated(DateTime.Today.ToString());
            newentry.setModified(DateTime.Today.ToString());
            MessageBox.Show(DateTime.Today.ToString());
            MessageBox.Show(newentry.getName());
          */
        }

        private void newEntryToolStripMenuItem1_Click(object sender, EventArgs e)
        {                                                       //to save space on screen, some panels overlay others to save space
            newentrygroupBox.Visible = true;                    //toggle panels overlaid each other
            newentrygroupBox.Enabled = true;                    //toggle panels overlaid each other

            if (editgroupBox.Visible == true)                   //toggle panels overlaid each other
            {
                editgroupBox.Visible = false;                   //toggle panels overlaid each other
                editgroupBox.Enabled = false;                   //toggle panels overlaid each other
            }

        }

        private void submitbutton_Click(object sender, EventArgs e) 
        {
            Application newentry = new Application();                       //create a new instance of the entry type
            newentry.setName(entrynametextBox.Text);                        //populate the new enty field with textbox inputs
            newentry.setUsername(loginuser);
            newentry.setEntryuser(entryusernametextBox.Text);               //populate the new enty field with textbox inputs
            newentry.setEntrypass(encript(entrypasswdtextBox.Text));        //populate the new enty field with textbox inputs
            newentry.setDetail(detailtextBox.Text);                         //populate the new enty field with textbox inputs
            newentry.setType(entrytypecomboBox.Text);
            newentry.setCreated(DateTime.Today.ToShortDateString());        //inialise date field
            newentry.setUpdated(DateTime.Today.ToShortDateString());        //inialise date field
            //MessageBox.Show(DateTime.Today.ToString());                   //debugging use only
            //MessageBox.Show(newentry.getName());
            newentry.saveEntry();
        }


        private void showentries(string query, string order)                //show entries on screen
        {
            var connection = new SQLiteConnection();                        //create new connection instance
            connection.ConnectionString = "Data Source=passman.db";         //state which database we are using
            connection.Open();                                              //open connection
            
            SQLiteCommand comm = new SQLiteCommand("Select * from entries  WHERE username = @q " + @order, connection); //create SQL query
            comm.Parameters.AddWithValue("@q", query);                      //assigng incomming values to placeholder
            entrydataGridView.Rows.Clear();                                 //Clear and refresh screen display table
            entrydataGridView.Refresh();                                    //Clear and refresh screen display  table


            using (SQLiteDataReader read = comm.ExecuteReader())            //create reader to read data from table
            {
                while (read.Read())                                         //read all the data
                {
                entrydataGridView.Rows.Add(new object[] {                   //create new row in display grid 
               
                read.GetValue(read.GetOrdinal("entryname")),                //read value from database table and assing it to grid column
                read.GetValue(read.GetOrdinal("entryusername")),             //read value from database table and assing it to grid column
                read.GetValue(read.GetOrdinal("entrypasswd")),               //read value from database table and assing it to grid column
                read.GetValue(read.GetOrdinal("extradetail")),               //read value from database table and assing it to grid column
                read.GetValue(read.GetOrdinal("entrytype")),                 //read value from database table and assing it to grid column
                read.GetValue(read.GetOrdinal("created")),                   //read value from database table and assing it to grid column
                read.GetValue(read.GetOrdinal("modified")),                  //read value from database table and assing it to grid column
            });
                }
            }

        }

        private void signUpbutton_Click(object sender, EventArgs e)         //activate singing up
        {
            confirmPwlabel.Enabled = true;                                  //display identifying label
            confirmPwtextBox.Enabled = true;                                //display identifying label
            randomPwbutton.Enabled = true;                                  //display identifying label
        }

        private void button3_Click(object sender, EventArgs e)              //call function to generate random password
        {
            string randompw = randompasswd();                               //assign the random value to a variable
            newuserPasswd1textBox.Text = randompw;                          //update text box with random value
            confirmPwtextBox.Text = randompw;                               //update text box with random value

        }


        private void confirmPwtextBox_DoubleClick(object sender, EventArgs e)   //confirm password
        {
            MessageBox.Show(loginpasswordBox.Text);                             //display connfirmed password
        }

        private void signUpToolStripMenuItem_Click(object sender, EventArgs e) //control menu strip
        {
            if (logingroupBox.Enabled == false)                                // menu options accordng to current state
            {
                signUpToolStripMenuItem.Text = "Sign Up";                      // menu options accordng to current state
                signUpgroupBox.Visible = false;                                 // menu options accordng to current state
                signUpgroupBox.Enabled = false;                                     // menu options accordng to current state
                logingroupBox.Visible = true;                                   // menu options accordng to current state
                logingroupBox.Enabled = true;                                   // menu options accordng to current state
            }
            else
            {
                signUpToolStripMenuItem.Text = "Login";                         // menu options accordng to current state
                signUpgroupBox.Visible = true;                                  // menu options accordng to current state
                signUpgroupBox.Enabled = true;                                 // menu options accordng to current state
                logingroupBox.Visible = false;                                  // menu options accordng to current state
                logingroupBox.Enabled = false;                                  // menu options accordng to current state
            }
            
            

        }

        private void showPwbutton_Click(object sender, EventArgs e)                 //display password
        {
            newuserPasswd1textBox.UseSystemPasswordChar = false;                    //clear password hiding
            confirmPwtextBox.UseSystemPasswordChar = false;                         //clear password hiding
        }

        private void newuserSubmitbutton_Click(object sender, EventArgs e)          //function to create new user
        {
            if (newuserPasswd1textBox.Text == confirmPwtextBox.Text)                //Check that password is confirmed
                {
                if (newUsernametextBox.Text.Length >= usrnamminlength)              //Check if the username meets minim lenth
                {
                     
                    User newuser = new User();                                      //Create new 'user' object
                    if (!usernameExists(newUsernametextBox.Text))                   //Check if the username already exists (must be unique)
                    {
                        newuser.setUsername(newUsernametextBox.Text);               //Set username for new user
                        newuser.setPassword(encript(newuserPasswd1textBox.Text));   //Encript and store password for new user
                        newuser.setAccount("standard");                             //Set the user account type (priviages)
                        newuser.addUseraccount();                                   //If all conditions are met, then save to database
                        signUpgroupBox.Enabled = false;                             //Close the signup controls if successful
                        signUpgroupBox.Visible = false;                             //Hide the signup controls if successful
                    }
                    else
                    {
                        MessageBox.Show("Username already taken");                  //flag-up username already exists
                        return;                                                     //drop out if so
                    }
                }
                else
                {
                    MessageBox.Show("Username must be at least " + usrnamminlength.ToString() + " characters long");// Report if if username is too short
                    return;
                }
            }
            else
            {
                MessageBox.Show("passwords do NOT match");                          //report if password comparison fails
                return;
            }   
        }

      
        private void randomPwbutton_Click(object sender, EventArgs e)               //call random password
        {
            
            entrypasswdtextBox.Text = randompasswd();                               //call password generator and assing it to text box
        }

        private void showPwbutton_Click_1(object sender, EventArgs e)               //show/hide password
        {
           if(entrypasswdtextBox.UseSystemPasswordChar == true)                     //show/hide password
                entrypasswdtextBox.UseSystemPasswordChar = false;                   //show/hide password
            else
                entrypasswdtextBox.UseSystemPasswordChar = true;                    //show/hide password
        }
   
        private void listAllToolStripMenuItem_Click(object sender, EventArgs e)     //menu option to show users
        {
            showentries(loginuser, "");                                             //list all entries for user currentrly logged-in
        }

        private void lastModifiedDescToolStripMenuItem_Click(object sender, EventArgs e)//order options to display entries
        {
            showentries(loginuser, "ORDER BY modified DESC");                       //show entries in decending order
        }

        private void lastModifiedToolStripMenuItem_Click(object sender, EventArgs e)//order options to display entries
        {
            showentries(loginuser, "ORDER BY modified ASC");                        //show entries in ascending order
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)        //control menu strip accordingly
        {
            editgroupBox.Visible = true;                                            //toggle visibilty of groupbaox panel
            editgroupBox.Enabled = true;                                            //toggle visibilty of groupbaox panel

            if (newentrygroupBox.Visible == true)                                   //toggle visibilty of groupbaox panel
            {
                newentrygroupBox.Visible = false;                                   //toggle visibilty of groupbaox panel
                newentrygroupBox.Enabled = false;                                   //toggle visibilty of groupbaox panel
            }
        }

        private void editsearchbutton_Click(object sender, EventArgs e)             //search for entry to edit
        {
            var connection = new SQLiteConnection();                                //create a new SQL connection
            connection.ConnectionString = "Data Source=passman.db";                 //assign the correct database

            connection.Open();                                                      //open the connection to the database

            SQLiteCommand sqlite_edit;                                              //create a command instance
            sqlite_edit = connection.CreateCommand();                               //create the command connection
            sqlite_edit.CommandText = @"select entryusername, entrypasswd,extradetail,modified,created, entrytype FROM entries WHERE username = @un AND entryname = @en ";
            sqlite_edit.Parameters.AddWithValue("@en", editsearchtextBox.Text);     //assign external values to paceholders for above query
            sqlite_edit.Parameters.AddWithValue("@un", loginuser);                  //assign external values to paceholders for above query

            using (var reader = sqlite_edit.ExecuteReader())                        //execute the query

                if(reader.Read())                                                   //was there anything to read?
                {
                    editentryusernametextBox.Text = reader["entryusername"].ToString();     //assign values read to text box for editing
                    editpasswordtextBox.Text = decript(reader["entrypasswd"].ToString());   //assign values read to text box for editing
                    editdetailtextBox.Text = reader["extradetail"].ToString();              //assign values read to text box for editing
                    editmodifiedtextBox.Text = reader["modified"].ToString();               //assign values read to text box for editing
                    editcreatedtextBox.Text = reader["created"].ToString();                 //assign values read to text box for editing
                    editdeletebutton.Enabled = true;                                        // if found enable 'delete' button
                    editupdatebutton.Enabled = true;                                        //if found enable 'update' button
                }

           else
            MessageBox.Show("Entry Not Found");                                             //if not found report

            connection.Close();                                                             //don't leave connection open
        }

        private void button3_Click_1(object sender, EventArgs e)                            //allow show/hide password
        {
            if (editpasswordtextBox.UseSystemPasswordChar == true)                          //hide password
                editpasswordtextBox.UseSystemPasswordChar = false;                          //show passwordpassword
            else
                editpasswordtextBox.UseSystemPasswordChar = true;                           //hide password
        }

        private void editdeletebutton_Click(object sender, EventArgs e)                     //confirm deletion of entry
        {
            DialogResult delete = MessageBox.Show("Are You Sure:", "Delete Entry", MessageBoxButtons.YesNo);//confirm deletion of entry
            if (delete == DialogResult.Yes)                                                 //confirm deletion of entry
            {
                deleteEntry(editsearchtextBox.Text, loginuser);                             //delete entry

                
            }
            else if (delete == DialogResult.No)                                             //don't delete entry 
            {
                return;                                                                     //quit
            }
        }

        private void editupdatebutton_Click(object sender, EventArgs e)                     //update entry
        {
            newentry.setEntrypass(editpasswordtextBox.Text);                                //assign values to entry object
            newentry.setEntryuser(editentryusernametextBox.Text);                           //assign values to entry object
            newentry.setName(editsearchtextBox.Text);                                       //assign values to entry object
            newentry.setDetail(editdetailtextBox.Text);                                     //assign values to entry object
            newentry.setUsername(loginuser);                                                //assign values to entry object    
            newentry.setUpdated(DateTime.Today.ToShortDateString());                        //assign values to entry object
            newentry.updateEntry();                                                         //call update method
        }

        private void listUsersToolStripMenuItem_Click(object sender, EventArgs e)           //control menu strip accordingly
        {
            showusers();                                                                    //call method to list all users
        }

        private void editAndDeleteToolStripMenuItem_Click(object sender, EventArgs e)       //control menu strip accordingly
        {
            admingroupBox.Enabled = true;                                                   //control the visibilty of groupbox panels
            admingroupBox.Visible = true;                                                   //control the visibilty of groupbox panels
        }

        private void adminusersearchbutton_Click(object sender, EventArgs e)                //search users (admin only)
        {
            var connection = new SQLiteConnection();                                        //create ne SQL connection
            connection.ConnectionString = "Data Source=passman.db";                         //assign correct database

            connection.Open();                                                              //open connection

            SQLiteCommand sqlite_edit;                                                      //create new command instance
            sqlite_edit = connection.CreateCommand();                                       //create connectin to instance
            sqlite_edit.CommandText = ("select username, userpasswd, acctype FROM users WHERE username = @un"); //build SQL query
            
            sqlite_edit.Parameters.AddWithValue("@un", admineditsearchtextBox.Text);        //assign external value to placeholder

            using (var reader = sqlite_edit.ExecuteReader())                                //call reader method

                if (reader.Read())                                                          //anything to read?
                {
                    admineditsearchtextBox.Text = reader["username"].ToString();            //assign field read to appropriate text box
                    adminpwedittextBox.Text = decript(reader["userpasswd"].ToString());     //assign field read to appropriate text box
                    adminacctypetextBox.Text = reader["acctype"].ToString();                //assign field read to appropriate text box
                    adminuserupdatebutton.Enabled = true;                                   //enable 'update' button if found
                    adminuserdeletebutton.Enabled = true;                                   //enable 'delete' button if found
                }
                
                else
                    MessageBox.Show("User Not Found");                                      //report if not found

            connection.Close();                                                             //dont leave connection open
        }

        private void adminshowpwbutton_Click(object sender, EventArgs e)                    //allow show admin password
        {
            if(adminpwedittextBox.UseSystemPasswordChar == true)                            //hide password
                adminpwedittextBox.UseSystemPasswordChar = false;                           //show password
            else
                adminpwedittextBox.UseSystemPasswordChar = true;                            //hide password
        }

        private void adminuserdeletebutton_Click(object sender, EventArgs e)                //confirm deletion of user
        {
            DialogResult deleteuser = MessageBox.Show("Are You Sure:", "Delete User " + admineditsearchtextBox.Text, MessageBoxButtons.YesNo);
            if (deleteuser == DialogResult.Yes)
            {
                deleteUser(admineditsearchtextBox.Text);                                    //delete user
            }
            else if (deleteuser == DialogResult.No)
            {
                return;                                                                     //don't delet user
            }
        }

        private void adminuserupdatebutton_Click(object sender, EventArgs e)                //user update (admin only)
        {        
                var connection = new SQLiteConnection();                                    //create new connection
                connection.ConnectionString = "Data Source=passman.db";                     //assign correct database

                connection.Open();                                                          //open connection to database
                if (connection.State == ConnectionState.Open)
                {
                    SQLiteCommand sqlite_useredit;                                          //create SQL command instance
                    sqlite_useredit = connection.CreateCommand();                            //create SQL connection command instance            
                sqlite_useredit.CommandText = ("UPDATE users SET userpasswd = @usrpasswd , acctype = @acctyp WHERE username = @un"); //SQL query
                    sqlite_useredit.Parameters.AddWithValue("@usrpasswd", encript(adminpwedittextBox.Text)); //assing external values to placeholder
                    sqlite_useredit.Parameters.AddWithValue("@acctyp", adminacctypetextBox.Text);           //assing external values to placeholder
                sqlite_useredit.Parameters.AddWithValue("@un", admineditsearchtextBox.Text);                //assing external values to placeholder
                sqlite_useredit.ExecuteNonQuery();                                                          //execute query
            }
                connection.Close();                                                             //don't leave connection open
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            usergroupBox.Visible = false;
        }

        private void showpasswordLoginCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (showpasswordLoginCheckBox.Checked == true)
            {
                showpasswordLoginCheckBox.Text = "Hide";                                         // This function shows and hides the password from the user if the check box is ticked
                loginpasswordBox.UseSystemPasswordChar = false;
            }
            else
            {
                showpasswordLoginCheckBox.Text = "Show";
                loginpasswordBox.UseSystemPasswordChar = true;
            }
        }

        private void showpasswordnewuser1CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (showpasswordnewuser1CheckBox.Checked == true)
            {
                showpasswordnewuser1CheckBox.Text = "Hide";                                          // This function shows and hides the password from the user if the check box is ticked
                newuserPasswd1textBox.UseSystemPasswordChar = false;
            }
            else
            {
                showpasswordnewuser1CheckBox.Text = "Show";
                newuserPasswd1textBox.UseSystemPasswordChar = true;
            }
        }

        private void showpasswordconfirmCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (showpasswordconfirmCheckBox.Checked == true)                                        // This function shows and hides the password from the user if the check box is ticked
            {
                showpasswordconfirmCheckBox.Text = "Hide";
                confirmPwtextBox.UseSystemPasswordChar = false;
            }
            else
            {
                showpasswordconfirmCheckBox.Text = "Show";
                confirmPwtextBox.UseSystemPasswordChar = true;
            }
        }

        private void entrydataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

