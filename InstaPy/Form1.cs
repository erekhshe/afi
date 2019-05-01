using System;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Text;

namespace InstaPy
{

    public partial class Form1 : Form
    {

        string[] InstaPyfiles = { "__init.py", "clariafai.py", "comment_util.py", "instapy.py", "login_util.py", "like_util.py", "print_log_writer.py", "time_util.py", "unfollow_util.py" };
        bool nostart = false;
        string FILENAME = "quickstart.py";
        bool cant = true;
        bool cant2 = true;
        bool cant3 = true;
        bool cant4 = true;
        bool cant5 = true;
        char[] charsToTrim = { ',', ' ' };
        private bool False;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            this.SetAutoScrollMargin(0, 60);

            string path = Directory.GetCurrentDirectory() + @"\instapy";

            if (Directory.Exists(path))
            {
                foreach (var item in InstaPyfiles)
                {
                    if (!File.Exists(path + @"\" + item))
                    {
                        missing.Text = "Some InstaPy files are missing.";
                        missing.ForeColor = System.Drawing.Color.Red;
                        nostart = false;
                        break;
                    }
                }
                path = Directory.GetCurrentDirectory() + @"\assets";
                if (!File.Exists(path + @"\chromedriver.exe"))
                {
                    missing.Text = "Chromedriver is missing.";
                    missing.ForeColor = System.Drawing.Color.Red;
                    nostart = false;

                }
                else
                {
                    nostart = true;
                    missing.Text = "All InstaPy files are there.";
                    missing.ForeColor = System.Drawing.Color.Green;
                }
            }
            else
            {
                nostart = false;
                missing.Text = "instapy folder is missing.";
                missing.ForeColor = System.Drawing.Color.Red;
            }


            // Shows intro message to read all info / usage of the program
            if (Properties.Settings.Default.readmedont)
            {
                MessageBox.Show("Please read info/usage before any program start.", "Read me", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (!Properties.Settings.Default.usernpass.Equals(string.Empty))
            {

                string[] del = { };
                del = Properties.Settings.Default.usernpass.Split('|');
                username_txt.Text = del[0];
                pass_txt.Text = del[1];
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Shows About page for program
            About bout = new About();
            bout.Show();
            //MessageBox.Show("GUI Tool for InstaPy script."+'\n'+'\n' + "Vesrion: 1.0" + '\n' + "Built in C#." + '\n' + "MIT License." + '\n'+'\n' +"InstaPy is utomation Script for \"farming\" Likes, Comments and Followers on Instagram." + '\n' + "Implemented in Python using the Selenium module." + '\n' + "MIT License.", "About",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }


        // this part write by KML

        #region RemoveIllegalCharZ

        /// <RemoveIllegalCharZ>
        ///  Remove Illegal char from string
        /// </RemoveIllegalCharZ>
        private string RemoveIllegalCharZ(string strContentIllgalChar)
        {
            string strWithoutIllegalCharZ = "";
            foreach (var item in strContentIllgalChar)
            {
                if ((int)item > 31)
                {
                    strWithoutIllegalCharZ = strWithoutIllegalCharZ.ToString() + item.ToString();
                }
            }
            return strWithoutIllegalCharZ.ToString().Trim();
        }
        #endregion

        private void button1_Click_1(object sender, EventArgs e)
        {
            Boolean bApplyDefaultFiltering = false;
            if (Array.Exists(Environment.GetCommandLineArgs(), element => element == "-filtering")) bApplyDefaultFiltering = true;

            string runPath, logPath;
            runPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\";
            logPath = runPath.Replace(@"\", @"\\");

            if (!nostart)
            {
                MessageBox.Show("Some files are missing!! Download all files from File Menu.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); //changed line
            }
            else
            {
                if (use_proxy.Checked)
                {
                    #region IMPORT
                    // Import part of python file
                    string import = "# -*- coding: utf-8 -*-" + '\n' +
                                    "import time" + '\n' +
                                    "from instapy import InstaPy" + '\n' +
                                    "from instapy.util import smart_run" + '\n' +                                    
                                    "from proxy_extension import create_proxy_extension" + '\n' +
                                    "from instapy import set_workspace" + '\n' +
                                    "from instapy import get_workspace" + '\n' +
                                    '\n' + '\n' +
                                    "set_workspace(path=\"" + logPath + "\")" + '\n' +
                                    '\n';
                    File.WriteAllText(FILENAME, import);
                    #endregion
                }
                else
                {
                    #region IMPORT
                    // Import part of python file
                    string import = "# -*- coding: utf-8 -*-" + '\n' +
                                    "import time" + '\n' +
                                    "from instapy import InstaPy" + '\n' +                                   
                                    "from instapy.util import smart_run" + '\n' +
                                    "from instapy import set_workspace" + '\n' +
                                    "from instapy import get_workspace" + '\n' +
                                    '\n' + '\n' +
                                    "set_workspace(path=\"" + logPath + "\")" + '\n' +
                                    '\n';
                    File.WriteAllText(FILENAME, import);
                    #endregion
                }

                #region USRNAME N PASSWORD


                // Username and Password processing 
                string usernpass = "";
                if (username_txt.Text.Equals(string.Empty) || pass_txt.Text.Equals(string.Empty))
                {
                    // Show error message if some of the field is empty
                    MessageBox.Show("ERROR: Username or Password are empty !", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // Set focus to Username
                    username_txt.Focus();
                    cant = false;
                }
                else
                {
                    if (use_proxy.Checked)
                    {
                        cant = true;

                        // If there is something in textboxes fill in line for sesion and login
                        username_txt.Text = RemoveIllegalCharZ(username_txt.Text);
                        pass_txt.Text = RemoveIllegalCharZ(pass_txt.Text);
                        pass_txt.Text = pass_txt.Text.Replace(@"'", @"\'");             // KML for ' in password
                        string proxy, strProxyWithUserPassCommand;
                        if (txtProxyUserName.Text != string.Empty || txtProxyPassword.Text != string.Empty)
                        {
                            proxy = "proxy = '" + txtProxyUserName.Text + ":" + txtProxyPassword.Text + "@" + proxy_ip.Text + ":" + proxy_port.Text + "'" + '\n'
                                    + "proxy_chrome_extension = create_proxy_extension(proxy)";
                            strProxyWithUserPassCommand = proxy + '\n' +
                                "session = InstaPy(username='" + username_txt.Text + "', password='" + pass_txt.Text + "', proxy_chrome_extension = proxy_chrome_extension" + ")" + '\n' +
                            '\n' +
                            "with smart_run(session):" + '\n' +
                            '\n';
                            
                            File.AppendAllText(FILENAME, strProxyWithUserPassCommand);
                        }
                        else
                        {
                            usernpass = "session = InstaPy(username='" + username_txt.Text + "', password='" + pass_txt.Text + "', proxy_address='" + proxy_ip.Text.ToString() + "', proxy_port='" + proxy_port.Text.ToString() + "', headless_browser=False" + ")" + '\n' +
                            '\n' +
                            "with smart_run(session):" + '\n' +
                            '\n';
                            File.AppendAllText(FILENAME, usernpass);
                        }


                        if (bApplyDefaultFiltering)
                        {
                            string ratio = "    session.set_relationship_bounds(enabled=True, potency_ratio=1.1, delimit_by_numbers=True, max_followers=4500, min_followers=30, min_following=50)" +
                                           '\n';
                            File.AppendAllText(FILENAME, ratio);
                        }
                        else
                        {
                            string ratio = "    session.set_relationship_bounds(enabled=False)" +
                                       '\n';
                            File.AppendAllText(FILENAME, ratio);
                        }
                    }
                    else
                    {
                        cant = true;

                        // If there is something in textboxes fill in line for sesion and login
                        username_txt.Text = RemoveIllegalCharZ(username_txt.Text);
                        pass_txt.Text = RemoveIllegalCharZ(pass_txt.Text);
                        pass_txt.Text = pass_txt.Text.Replace(@"'", @"\'");           // KML for ' in password

                        usernpass = "session = InstaPy(username='" + username_txt.Text + "', password='" + pass_txt.Text + "', headless_browser=False" + ")" + '\n' +
                                    '\n' +
                                    "with smart_run(session):" + '\n' +
                                    '\n';

                        File.AppendAllText(FILENAME, usernpass);
                        if (bApplyDefaultFiltering)
                        {
                            string ratio = "    session.set_relationship_bounds(enabled=True, potency_ratio=1.1, delimit_by_numbers=True, max_followers=4500, min_followers=30, min_following=50)" +
                                           '\n';
                            File.AppendAllText(FILENAME, ratio);
                        }
                        else
                        {
                            if (upperfc.Checked | lowerfc.Checked)
                            {
                                if (!upperfc.Checked) { upperfc_percent.Value = 0; };
                                if (!lowerfc.Checked) { lowerfc_percent.Value = 0; };
                                if (upperfc_percent.Value > lowerfc_percent.Value)
                                {
                                    string ratio = "    session.set_relationship_bounds(enabled=False, delimit_by_numbers=True, max_followers=" + upperfc_percent.Value.ToString() + ", min_followers=" + lowerfc_percent.Value.ToString() + ")" +
                                               '\n';
                                    File.AppendAllText(FILENAME, ratio);
                                }
                            }
                            else
                            {
                                string ratio = "    session.set_relationship_bounds(enabled=False, delimit_by_numbers=False)" +
                                           '\n';
                                File.AppendAllText(FILENAME, ratio);
                            }
                        }
                    }

                }
                if (remember.Checked)
                {
                    string settingsuser = username_txt.Text + "|" + pass_txt.Text;
                    Properties.Settings.Default.usernpass = settingsuser;
                    Properties.Settings.Default.Save();
                }
                #endregion


                #region Skipping user for private account, no profile picture, business account
                //This is done by default
                //session.set_skip_users(skip_private = True, private_percentage = 100)

                if (bApplyDefaultFiltering)
                {
                    string skiping = string.Empty; ;

                    if (rdbPrivateUser.Checked == true)
                    {
                        skiping = "    session.set_skip_users(skip_private=False," +
                            " private_percentage=100," +
                            " skip_no_profile_pic=True," +
                            " no_profile_pic_percentage=100," +
                            " skip_business=True," +
                            " business_percentage=80)" + '\n';
                    }
                    else if (rdbNonPrivateUser.Checked == true)
                    {
                        skiping = "    session.set_skip_users(skip_private=True," +
                            " private_percentage=100," +
                            " skip_no_profile_pic=True," +
                            " no_profile_pic_percentage=100," +
                            " skip_business=True," +
                            " business_percentage=80)" + '\n';
                    }

                    File.AppendAllText(FILENAME, skiping);
                }
                else
                {
                    string skiping = string.Empty;
                    if (chbFillter.Checked == true)
                    {
                        if (rdbPrivateUser.Checked == true)
                        {
                            skiping = "    session.set_skip_users(skip_private=False)" + '\n';
                        }
                        else if (rdbNonPrivateUser.Checked == true)
                        {
                            skiping = "    session.set_skip_users(skip_private=True)" + '\n';
                        }
                    }
                    else
                    {
                        skiping = "    session.set_skip_users(skip_private=False)" + '\n';
                    }
                     
                    File.AppendAllText(FILENAME, skiping);
                }
                #endregion

                #region Custom action delays
                //This is done by default
                //session.set_skip_users(skip_private = True, private_percentage = 100)

                if (1==1)
                {
                    string actionDelays = string.Empty; ;

                    actionDelays = "    session.set_action_delays(enabled=True, like = 40, comment = 80, follow = 30, unfollow = 60, randomize = True, random_range = (50, 100))" + '\n';
                    //actionDelays = "    session.set_action_delays(enabled=True, like = 40, comment = 80, follow = 40, unfollow = 60, randomize = True, random_range = (50, 100))" + '\n';
                    File.AppendAllText(FILENAME, actionDelays);
                }
         
                #endregion

                #region Ignoring Users

                if (chkIgnoringUsers.Checked)
                {
                    string users = "['";
                    string[] user = {};
                    if (!txtIgnorUsersList.Text.Equals(string.Empty))
                    {
                        txtIgnorUsersList.Text = RemoveIllegalCharZ(txtIgnorUsersList.Text);
                        user = txtIgnorUsersList.Text.Trim(charsToTrim).Trim(charsToTrim).Split(',');

                        foreach (var item in user)
                        {
                            string item2 = item.Trim();
                            item2 += "\',\'";
                            users += item2.Trim();
                        }
                        users = users.Remove(users.Length - 2, 2) + "]";
                    }
                    else
                        MessageBox.Show("Username is empty.", "Ignoring user error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    string IgnoringUsers = "    session.set_ignore_users(" + users + ")" + '\n';
                    File.AppendAllText(FILENAME, IgnoringUsers);
                }

                #endregion

                #region Blacklist Campign
                if (block_campaign.Checked)               
                 {
                    //# Controls your interactions by campaigns.
                    //# ex. this week InstaPy will like and comment interacting by campaign called
                    //# 'soccer', next time InstaPy runs, it will not interact again with users in
                    //# blacklist
                    //# In general, this means that once we turn off the soccer_campaign again, InstaPy
                    //# will have no track of the people it interacted with about soccer.
                    //# This will help you target people only once but several times for different campaigns

                    //session.set_blacklist(enabled=True, campaign='soccer_campaign')

                    
                    string cmms = "'";
                    string[] cmm = { };
                    if (!title_of_campaign.Text.Equals(string.Empty))
                    {
                        cmm = title_of_campaign.Text.Trim(charsToTrim).Split(',');

                        foreach (var item in cmm)
                        {
                            string item2 = item.Trim();
                            item2 += "\',\'";
                            cmms += item2.Trim();
                        }
                        cmms = cmms.Remove(cmms.Length - 2, 2);

                        string comment = "    session.set_blacklist(enabled=True, campaign=" + cmms + ")" + '\n';
                        if (comment.StartsWith(","))
                        {
                            comment = comment.Remove(comment.Length - 0, 1);
                        }
                        File.AppendAllText(FILENAME, comment);

                    }
                }
                #endregion

                #region Interactions based on the number of posts a user has
                //--
                #endregion

                #region Liking based on the number of existing likes a post has
                if (liking_based_on_likes.Checked)
                {
                    string liking = "    session.set_delimit_liking(enabled=True, max=" + max_likes.Value.ToString() + ", min=" + min_likes.Value.ToString() + " )" + '\n';
                    File.AppendAllText(FILENAME, liking);
                }
                #endregion

                #region Specialized comments for images with specific content
                //# checks the image for keywords food and lunch, if both are found,
                //# comments with the given comments. If full_match is False (default), it only
                //# requires a single tag to match Clarifai results.
                if (specific_content.Checked)
                {
                    string tags = "['";
                    string[] tag = { };
                    if (!desired_content.Text.Equals(string.Empty))
                    {
                        tag = desired_content.Text.Trim(charsToTrim).Split(',');

                        foreach (var item in tag)
                        {
                            string item2 = item.Trim();
                            item2 += "\',\'";
                            tags += item2.Trim();
                        }
                        tags = tags.Remove(tags.Length - 2, 2) + "]";
                    }

                    else MessageBox.Show("Write down some users.", "ATTENTION", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    string specifiedcontent = "    session.clarifai_check_img_for(" + tags + ", comment=True)" + '\n';
                    if (specifiedcontent.StartsWith(","))
                    {
                        specifiedcontent = specifiedcontent.Remove(specifiedcontent.Length - 0, 1);
                    }
                    File.AppendAllText(FILENAME, specifiedcontent);

                }
                #endregion

                #region Don't unfollow active users
                //Prevents unfollow followers who have liked one of your latest 5 posts
                if (unfollow_check.Checked)
                {
                    string unfollowactiveusers = "    session.set_dont_unfollow_active_users(enabled=True, posts=" + number_of_posts.Value.ToString() + ")" + '\n';
                    File.AppendAllText(FILENAME, unfollowactiveusers);
                }
                #endregion

                #region Commenting
                /*=============================================================================================
				 *			    Commenting
				 *				Enables commenting 
				 *				Set percentage and custom comments
				 *=============================================================================================*/

                string commentLine = "    session.set_do_comment(enabled=True, percentage=";
                string commentSetLine = "    session.set_comments([";
                string[] comments = { };
                if (comment.Checked)
                {
                    // Set percentage for commenting
                    commentLine += comment_percent.Value.ToString() + ")" + '\n';

                    if (comment_cust_txt.Text.Equals(string.Empty))
                    {
                        // MessageBox is shown
                        MessageBox.Show("ERROR: No comments detected. Write some comments or deselect the option.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        // Comments field gets in focus to type
                        comment_cust_txt.Focus();
                    }
                    else
                    {
                        comment_cust_txt.Text = RemoveIllegalCharZ(comment_cust_txt.Text);
                        comments = comment_cust_txt.Text.Trim(charsToTrim).Trim(charsToTrim).Split(',');

                        // Goes for every comment to add in string
                        foreach (var item in comments)
                        {
                            // If there is empty comment caused with accident comma (ie. " nice, ") just continue
                            if (item.Equals(string.Empty))
                            {
                                continue;
                            }
                            // If there is comment add it in line 
                            else
                            {
                                if (emojisupport.Checked)
                                {
                                    commentSetLine += item.Trim() + "'";
                                    if (comments[comments.Length - 1] != item)
                                        commentSetLine += "'," + '\n' + "                          ";
                                }
                                else
                                {
                                    commentSetLine += "u'" + item.Trim() + "'";
                                    if (comments[comments.Length - 1] != item)
                                        commentSetLine += "," + '\n' + "                          ";
                                }
                            }
                        }
                        // Removes processed comma to add closing bracket
                        commentSetLine = commentSetLine.Remove(commentSetLine.Length - 1, 1) + "'])" + '\n';
                        if (commentSetLine.StartsWith(","))
                        {
                            commentSetLine = commentSetLine.Remove(commentSetLine.Length - 0, 1);
                        }
                        File.AppendAllText(FILENAME, commentLine);
                        File.AppendAllText(FILENAME, commentSetLine);
                    }
                }
                #endregion

                #region Following
                /*===============================================================================================
				 *			Following
				 *				Set percentage to follow every x/100th user
				 * 
				 * =============================================================================================*/
                if (following.Checked)
                {
                    // Set following to true and read percent and times
                    string follow = "    session.set_do_follow(enabled=True, percentage="
                        + following_percent.Value.ToString() + ", times="
                        + followtimes.Value.ToString() + ")" + '\n';

                    File.AppendAllText(FILENAME, follow);
                }
                #endregion

                #region Following by a list
                /*========================================================================
				 *			Follow from list
				 * 
				 * ========================================================================*/
                if (followfromlist.Checked)
                {
                    string accs = "['";
                    string[] acc = { };
                    if (!following_list.Text.Equals(string.Empty))
                    {
                        acc = following_list.Text.Trim(charsToTrim).Split(',');

                        foreach (var item in acc)
                        {
                            string item2 = item.Trim();
                            item2 += "\',\'";
                            accs += item2.Trim();
                        }
                        accs = accs.Remove(accs.Length - 2, 2) + "]";

                        string followfromlist = "    session.follow_by_list(followlist=" + accs + ", times=1, sleep_delay=600, interact=False)" + '\n';

                        if (followfromlist.StartsWith(","))
                        {
                            followfromlist = followfromlist.Remove(followfromlist.Length - 0, 1);
                        }
                        File.AppendAllText(FILENAME, followfromlist);
                    }
                }
                #endregion

                #region Follow someone else's followers
                if (FSEF_check.Checked)
                {
                    string fsef = "    session.follow_user_followers([";
                    string[] fsef_users = { };

                    if (FSEF_txt.Text.Equals(string.Empty))
                    {

                        MessageBox.Show("ERROR: No users detected. Write some users or deselect this option.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        cant4 = false;
                        FSEF_txt.Focus();

                    }
                    else
                    {
                        fsef_users = FSEF_txt.Text.Trim(charsToTrim).Split(',');
                        cant4 = true;
                    }
                    foreach (var item in fsef_users)
                    {
                        // If there is emtpy tag skip it
                        if (item.Equals(string.Empty))
                        {
                            continue;
                        }
                        else fsef += "'" + item.Trim() + "', ";
                    }
                    fsef = fsef.Remove(fsef.Length - 2, 2) + "]";
                    fsef += ", amount = " + fsef_amount.Value.ToString() + ",";

                    if (fsef_rand.Checked)
                    {
                        fsef += " randomize=True";
                    }
                    else fsef += " randomize=False";

                    if (fsef_delay.Value.ToString() != "600")
                    {
                        fsef += ", sleep_delay=" + fsef_delay.Value.ToString();
                    }
                    fsef += ", interact=False)" + '\n';
                    if (fsef.StartsWith(","))
                    {
                        fsef = fsef.Remove(fsef.Length - 0, 1);
                    }
                    File.AppendAllText(FILENAME, fsef);
                }
                #endregion

                #region Follow users that someone else is following

                if (fusef_ch.Checked)
                {
                    string fusef = "    session.follow_user_following([";
                    string[] fusef_users = { };

                    if (fusef_txt.Text.Equals(string.Empty))
                    {

                        MessageBox.Show("ERROR: No users detected. Write some users or deselect this option.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        cant5 = false;

                        fusef_txt.Focus();

                    }
                    else
                    {
                        fusef_users = fusef_txt.Text.Trim(charsToTrim).Split(','); cant5 = true;
                    }
                    foreach (var item in fusef_users)
                    {
                        // If there is emtpy tag skip it
                        if (item.Equals(string.Empty))
                        {
                            continue;
                        }
                        else fusef += "'" + item.Trim() + "', ";
                    }
                    fusef = fusef.Remove(fusef.Length - 2, 2) + "]";
                    fusef += ", amount = " + fusef_amoun.Value.ToString() + ",";

                    if (fusef_rand.Checked)
                    {
                        fusef += " randomize=True";
                    }
                    else fusef += " randomize=False";

                    if (fusef_delay.Value.ToString() != "600")
                    {
                        fusef += ", sleep_delay=" + fusef_delay.Value.ToString() + ")" + '\n';
                    }
                    else fusef += ")" + '\n';

                    if (fusef.StartsWith(","))
                    {
                        fusef = fusef.Remove(fusef.Length - 0, 1);
                    }
                    File.AppendAllText(FILENAME, fusef);

                }



                #endregion 

                #region Follow users that someone else is followers/following
                //---
                #endregion

                #region Follow by Tags
                /// Follow user based on hashtags (without liking the image)

                if (FollowByTags.Checked)
                {
                    string accs = "['";
                    string[] acc = { };
                    if (!tag_list.Text.Equals(string.Empty))
                    {
                        acc = tag_list.Text.Trim(charsToTrim).Split(',');

                        foreach (var item in acc)
                        {
                            string item2 = item.Trim();
                            item2 += "\',\'";
                            accs += item2.Trim();
                        }
                        accs = accs.Remove(accs.Length - 2, 2) + "]";

                        string FollowByTags = "    session.follow_by_tags(" + accs + ", amount=" + Numberoftags.Value.ToString() + ")" + '\n';

                        if (FollowByTags.StartsWith(","))
                        {
                            FollowByTags = FollowByTags.Remove(FollowByTags.Length - 0, 1);
                        }
                        File.AppendAllText(FILENAME, FollowByTags);
                    }

                }


                #endregion

                #region Follow the likers of photos of users
                if (Followthelikersofphotosofusers.Checked)
                {
                    string accs = "['";
                    string[] acc = { };
                    if (!user_photo_list.Text.Equals(string.Empty))
                    {
                        acc = user_photo_list.Text.Trim(charsToTrim).Split(',');

                        foreach (var item in acc)
                        {
                            string item2 = item.Trim();
                            item2 += "\',\'";
                            accs += item2.Trim();
                        }
                        accs = accs.Remove(accs.Length - 2, 2) + "]";
                        string followlikers = "    session.follow_likers(" + accs + ", photos_grab_amount = " + number_of_photos.Value.ToString() + ", follow_likers_per_photo = " + follow_likers_per_photo.Value.ToString() + ", randomize=False, sleep_delay=600, interact=False)" + '\n';
                        if (followlikers.StartsWith(","))
                        {
                            followlikers = followlikers.Remove(followlikers.Length - 0, 1);
                        }
                        File.AppendAllText(FILENAME, followlikers);
                    }
                    else MessageBox.Show("Write down some users.", "ATTENTION", MessageBoxButtons.OK, MessageBoxIcon.Warning);


                }
                #endregion

                #region Follow the commenters of photos of users

                if (follow_commenters_user.Checked)
                {
                    string accs = "['";
                    string[] acc = { };
                    if (!commenting_users.Text.Equals(string.Empty))
                    {
                        acc = commenting_users.Text.Trim(charsToTrim).Split(',');

                        foreach (var item in acc)
                        {
                            string item2 = item.Trim();
                            item2 += "\',\'";
                            accs += item2.Trim();
                        }
                        accs = accs.Remove(accs.Length - 2, 2) + "]";

                        string followbycomments = "    session.follow_commenters(" + accs + ", amount=" + follow_amount.Value.ToString() + ", daysold=" + number_of_days.Value.ToString() + ", max_pic=" + Max_pic_number.Value.ToString() + ", sleep_delay=600, interact=False)" + '\n';
                        if (followbycomments.StartsWith(","))
                        {
                            followbycomments = followbycomments.Remove(followbycomments.Length - 0, 1);
                        }
                        File.AppendAllText(FILENAME, followbycomments);
                    }

                }

                #endregion

                #region Interact with specific users
                // Interact with specific users
                // set_do_like, set_do_comment, set_do_follow are applicable
                if (Interact_with_users.Checked)
                {
                    string Comms = "['";
                    string[] comm = { };
                    if (!Set_comment.Text.Equals(string.Empty))
                    {
                        comm = Set_comment.Text.Trim(charsToTrim).Split(',');

                        foreach (var item in comm)
                        {
                            string item2 = item.Trim();
                            item2 += "\',\'";
                            Comms += item2.Trim();
                        }
                        Comms = Comms.Remove(Comms.Length - 2, 2) + "]";

                        string Comments = "    session.set_comments(" + Comms + ")" + '\n';

                        if (Comments.StartsWith(","))
                        {
                            Comments = Comments.Remove(Comments.Length - 0, 1);
                        }
                        File.AppendAllText(FILENAME, Comments);

                    }

                    string percentage = "    session.set_do_comment(enabled=True, percentage=80)" + '\n';
                    File.AppendAllText(FILENAME, percentage);

                    //string isu_dofollow = "    session.set_do_follow(enabled=True, percentage=50)" + '\n';
                    //File.AppendAllText(FILENAME, isu_dofollow);

                    if (like_media.Checked)
                    {
                        string Likemedia = "    session.set_do_like(True, percentage=70)" + '\n';
                        File.AppendAllText(FILENAME, Likemedia);


                    }
                    string accs = "['";
                    string[] acc = { };
                    if (!List_of_specific_users.Text.Equals(string.Empty))
                    {
                        acc = List_of_specific_users.Text.Trim(charsToTrim).Split(',');

                        foreach (var item in acc)
                        {
                            string item2 = item;
                            item2 += "\',\'";
                            accs += item2;
                        }
                        accs = accs.Remove(accs.Length - 2, 2) + "]";

                        string Likemedia = "    session.interact_by_users(" + accs + ", amount=" + number_of_comments.Value.ToString() + ", randomize=True)" + '\n';

                        if (Likemedia.StartsWith(","))
                        {
                            Likemedia = Likemedia.Remove(Likemedia.Length - 0, 1);
                        }
                        File.AppendAllText(FILENAME, Likemedia);

                    }
                }

                #endregion

                #region Interact with specific users' tagged posts
                //--
                #endregion

                #region Interact with users that someone else is following
                if (Intract_with_someoneelse_follower.Checked)
                {

                    string interactionamount = "    session.set_user_interact(amount=" + txtInteractionFollowingPostsAmount.Value.ToString() + ", randomize=True, percentage=100, media='Photo')" + '\n';

                    File.AppendAllText(FILENAME, interactionamount);

                    if (txtInteractionFollowingFollowPercentage.Value != 0)
                    {
                        string follow = "    session.set_do_follow(enabled=True, percentage=" + txtInteractionFollowingFollowPercentage.Value.ToString() + ")" + '\n';
                        File.AppendAllText(FILENAME, follow);
                    }

                    if (txtInteractionFollowingLikePercentage.Value != 0)
                    {
                        string like = "    session.set_do_like(enabled=True, percentage=" + txtInteractionFollowingLikePercentage.Value.ToString() + ")" + '\n';
                        File.AppendAllText(FILENAME, like);
                    }
                    
                    string cmms = "['";
                    string[] cmm = { };
                    
                    if ((!txtInteractionFollowingSetComment.Text.Equals(string.Empty)) && (txtInteractionFollowingSetComment.Text.Trim() != ","))
                    {
                        cmm = txtInteractionFollowingSetComment.Text.Trim(charsToTrim).Split(',');

                        foreach (var item in cmm)
                        {
                            //string item2 = item.Trim();
                            //item2 += "\',\'";
                            //cmms += item2.Trim();
                            if (item.Equals(string.Empty))
                            {
                                continue;
                            }
                            // If there is comment add it in line 
                            else
                            {
                                if (emojisupport.Checked)
                                {
                                    cmms += item.Trim() + "'";
                                    if (cmm[cmm.Length - 1] != item)
                                        cmms += "'," + '\n' + "                          ";
                                }
                                else
                                {
                                    cmms += "u'" + item.Trim() + "'";
                                    if (cmm[cmm.Length - 1] != item)
                                        cmms += "," + '\n' + "                          ";
                                }
                            }
                        }
                        cmms = cmms.Remove(cmms.Length - 1, 1) + "']";

                        string comment = "    session.set_comments(" + cmms + ")" + '\n';
                        if (comment.StartsWith(","))
                        {
                            comment = comment.Remove(comment.Length - 0, 1);
                        }
                        File.AppendAllText(FILENAME, comment);
                    }
                    else if (txtInteractionFollowingCommentsPercentage.Value != 0)
                    {
                        MessageBox.Show("Comment is empty", "ATTENTION", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    string docomment = "    session.set_do_comment(enabled=True, percentage=" + txtInteractionFollowingCommentsPercentage.Value.ToString() + ")" + '\n';

                    File.AppendAllText(FILENAME, docomment);


                    string accs = "['";
                    string[] acc = { };
                    if (!interact_user_following.Text.Equals(string.Empty))
                    {
                        acc = interact_user_following.Text.Trim(charsToTrim).Split(',');

                        foreach (var item in acc)
                        {
                            string item2 = item.Trim();
                            item2 += "\',\'";
                            accs += item2.Trim();
                        }
                        accs = accs.Remove(accs.Length - 2, 2) + "]";

                        string accounts = "    session.interact_user_following(" + accs + ", amount=" + number_of_user_interaction.Value.ToString() + ", randomize=False)" + '\n';
                        if (accounts.StartsWith(","))
                        {
                            accounts = accounts.Remove(accounts.Length - 0, 1);
                        }
                        File.AppendAllText(FILENAME, accounts);
                    }
                    else MessageBox.Show("Write down some users.", "ATTENTION", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                #endregion

                #region Interact with someone else's followers
                //Interact with the people that a given user is following
                //set_do_comment, set_do_follow and set_do_like are applicable
                if (interact_with_someone_else_followers.Checked)
                {
                    string interactionamount = "    session.set_user_interact(amount=" + txtInteractionFollowerPostsAmount.Value.ToString() + ", randomize=True, percentage=100, media='Photo')" + '\n';
                    File.AppendAllText(FILENAME, interactionamount);


                    if (txtInteractionFollowerFollowPercentage.Value != 0)
                    {
                        string follow = "    session.set_do_follow(enabled=True, percentage=" + txtInteractionFollowerFollowPercentage.Value.ToString() + ")" + '\n';
                        File.AppendAllText(FILENAME, follow);
                    }

                    if (txtInteractionFollowerLikePercentage.Value != 0)
                    {
                        string like = "    session.set_do_like(enabled=True, percentage=" + txtInteractionFollowerLikePercentage.Value.ToString() + ")" + '\n';
                        File.AppendAllText(FILENAME, like);
                    }

                    string cmms = "[";
                    string[] cmm = { };
                    if ((!txtInteractionFollowerSetComment.Text.Equals(string.Empty)) && (txtInteractionFollowerSetComment.Text.ToString().Trim() != ","))
                    {
                       // MessageBox.Show(txtInteractionFollowerSetComment.Text);
                        txtInteractionFollowerSetComment.Text = RemoveIllegalCharZ(txtInteractionFollowerSetComment.Text);
                        cmm = txtInteractionFollowerSetComment.Text.Trim(charsToTrim).Split(',');

                        foreach (var item in cmm)
                        {
                            //  string item2 = item.Trim();
                            //  item2 += "\',\'";
                            //  cmms += item2.Trim();

                            // If there is empty comment caused with accident comma (ie. " nice, ") just continue
                            if (item.Equals(string.Empty))
                            {
                                continue;
                            }
                            // If there is comment add it in line 
                            else
                            {
                                if (emojisupport.Checked)
                                {
                                    cmms += item.Trim() + "'";
                                    if (cmm[cmm.Length - 1] != item)
                                        cmms += "'," + '\n' + "                          ";
                                }
                                else
                                {
                                    cmms += "u'" + item.Trim() + "'";
                                    if (cmm[cmm.Length - 1] != item)
                                        cmms += "," + '\n' + "                          ";
                                }
                            }
                        }
                        cmms = cmms.Remove(cmms.Length - 1, 1) + "']";

                        string comment = "    session.set_comments(" + cmms + ")" + '\n';
                        if (comment.StartsWith(","))
                        {
                            comment = comment.Remove(comment.Length - 0, 1);
                        }
                        File.AppendAllText(FILENAME, comment);
                    }
                    else if (txtInteractionFollowerCommentsPercentage.Value != 0)
                    {
                        MessageBox.Show("Comment is empty", "ATTENTION", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }


                    string docomment = "    session.set_do_comment(enabled=True, percentage=" + txtInteractionFollowerCommentsPercentage.Value.ToString() + ")" + '\n';
                    File.AppendAllText(FILENAME, docomment);

                    string accs = "['";
                    string[] acc = { };
                    if (!desired_user.Text.Equals(string.Empty))
                    {
                        acc = desired_user.Text.Trim(charsToTrim).Split(',');

                        foreach (var item in acc)
                        {
                            string item2 = item.Trim();
                            item2 += "\',\'";
                            accs += item2.Trim();
                        }
                        accs = accs.Remove(accs.Length - 2, 2) + "]";

                        string accounts = "    session.interact_user_followers(" + accs + ", amount=" + user_interaction_count.Value.ToString() + ", randomize=False)" + '\n';
                        if (accounts.StartsWith(","))
                        {
                            accounts = accounts.Remove(accounts.Length - 0, 1);
                        }
                        File.AppendAllText(FILENAME, accounts);
                    }
                    else MessageBox.Show("Write down some users.", "ATTENTION", MessageBoxButtons.OK, MessageBoxIcon.Warning);


                }

                #endregion

                #region Interact on posts at given URLs
                //--
                #endregion

                #region Interact by Comments
                //--
                #endregion

                #region Unfollowing
                /*====================================================================================================
				 *			Unfollowing
				 *			#unfollows 10 of the accounts you're following -> instagram will only unfollow 10 before
				 *			you'll be 'blocked for 10 minutes'
				 *			
				 *			(if you enter a higher number than 10 it will unfollow 10,
				 *			then wait 10 minutes and will continue then)
				 *			
				 *======================================================================================================*/

                if (unfollow.Checked)
                {
                    /* # unfollows 10 of the accounts you're following -> instagram will only
                    # unfollow 10 before you'll be 'blocked for 10 minutes' (if you enter a
                    # higher number than 10 it will unfollow 10, then wait 10 minutes and will
                    # continue then).
                    # You can choose to only unfollow the user that Insta has followed by adding
                    # onlyInstapyFollowed = True otherwise it will unfollow all users
                    # You can choose unfollow method as FIFO (First-Input-First-Output) or
                    # LIFO (Last-Input-First-Output). The default is FIFO method.
                    # onlyInstapyMethod is using only when onlyInstapyFollowed = True
                    # sleep_delay sets the time it will sleep every 10 profile unfollow, default
                    # is 10min

                                        session.unfollow_users(amount = 10, onlyInstapyFollowed = True, onlyInstapyMethod = 'FIFO', sleep_delay = 60)

                    # You can only unfollow user that won't follow you back by adding
                    # onlyNotFollowMe = True it still only support on profile following
                    # you should disable onlyInstapyFollowed when use this
                    session.unfollow_users(amount = 10, onlyNotFollowMe = True, sleep_delay = 60) */

                    string unfollow = string.Empty;

                    if (rdbInstapyFollower.Checked == true)
                    {
                        // Unfollow the users WHO was followed by InstaPy (has 2 tracks- "all" and "nonfollowers"):  again, if you like to unfollow all of the users followed by InstaPy, use the track-"all";
                        unfollow = "    session.unfollow_users(amount=" + unfollow_nmbr.Value.ToString() + ", InstapyFollowed=(True, \"all\"), style=\"FIFO\", unfollow_after=90 * 60 * 60, sleep_delay=501)" + '\n';                   
                    }
                    else if (rdbNonFollower.Checked == true)
                    {
                        // Unfollow the users WHO do not follow you back:
                        //unfollow = "    session.unfollow_users(amount=" + unfollow_nmbr.Value.ToString() + ", InstapyFollowed = (True, \"nonfollowers\"), style = \"FIFO\", unfollow_after = 90 * 60 * 60, sleep_delay = 501" + ")" + '\n';

                        //but if you like you unfollow only the users followed by InstaPy WHO do not follow you back, use the track- "nonfollowers";
                        unfollow = "    session.unfollow_users(amount=" + unfollow_nmbr.Value.ToString() + ", nonFollowers=True, style=\"RANDOM\", unfollow_after=48*60*60, sleep_delay=655)" + '\n';
                    }
                    else if (rdbAllFollower.Checked == true)
                    {
                        // Just unfollow, regardless of a user follows you or not
                        unfollow = "    session.unfollow_users(amount=" + unfollow_nmbr.Value.ToString() + ", allFollowing=True, style=\"LIFO\", unfollow_after=3 * 60 * 60, sleep_delay=450)" + '\n';
                    }

                    File.AppendAllText(FILENAME, unfollow);
                }

                #endregion

                #region Commenting based on the number of existing comments a post has
                if (bApplyDefaultFiltering)
                {
                    //if (commenting_based_on_likes.Checked)
                    //{
                    //string commenting = "    session.set_delimit_commenting(enabled=True, max=" + max_comments.Value.ToString() + ", min=" + min_comments.Value.ToString() + " )" + '\n';
                    string commenting = "    session.set_delimit_commenting(enabled=True, max=200, min=0)" + '\n';
                    File.AppendAllText(FILENAME, commenting);
                    //}
                }
                #endregion

                #region Commenting based on madatory words in the description or first comment
                //--
                #endregion

                #region Comment by Locations
                if (comment_by_location.Checked)
                {
                    string cmms = "['";
                    string[] cmm = { };
                    if (!comment_locations.Text.Equals(string.Empty))
                    {
                        cmm = comment_locations.Text.Trim(charsToTrim).Split(',');

                        foreach (var item in cmm)
                        {
                            string item2 = item.Trim();
                            item2 += "\',\'";
                            cmms += item2.Trim();
                        }
                        cmms = cmms.Remove(cmms.Length - 2, 2) + "]";

                        string comment = "    session.comment_by_locations(" + cmms + ", amount=" + desired_amount.Value.ToString() + ")" + '\n';
                        if (comment.StartsWith(","))
                        {
                            comment = comment.Remove(comment.Length - 0, 1);
                        }
                        File.AppendAllText(FILENAME, comment);
                    }
                }
                #endregion

                #region Like by Locations
                /*========================================================================
				 *			Likes from location
				 * 
				 * ========================================================================*/

                if (likefromlocation.Checked)
                {
                    string cmms = "['";
                    string[] cmm = { };
                    if (!location_txt.Text.Equals(string.Empty))
                    {
                        cmm = location_txt.Text.Trim(charsToTrim).Split(',');

                        foreach (var item in cmm)
                        {
                            string item2 = item.Trim();
                            item2 += "\',\'";
                            cmms += item2.Trim();
                        }
                        cmms = cmms.Remove(cmms.Length - 2, 2) + "]";
                        cant4 = true;
                        string comment = "    session.like_by_locations(" + cmms + ", amount=" + location_nmb.Value.ToString() + ")" + '\n';
                        if (comment.StartsWith(","))
                        {
                            comment = comment.Remove(comment.Length - 0, 1);
                        }



                        File.AppendAllText(FILENAME, comment);


                    }
                }

                #endregion  

                #region Like by Tags
                /*========================================================================
				 *			Likes by tags
				 * Like by Tags
                    
                    # Like posts based on hashtags
                    session.like_by_tags(['natgeo', 'world'], amount=10)
                    Like by Tags and interact with user
                    # Like posts based on hashtags and like 3 posts of its poster
                    session.set_user_interact(amount=3, randomize=True, percentage=100, media='Photo')
                    session.like_by_tags(['natgeo', 'world'], amount=10, interact=True)
				 * ========================================================================*/

                if (likefromtags.Checked)
                {
                    string likesFromTagsLine = "    session.like_by_tags([";
                    string[] tags = { };
                    if (likesfromtags_txt.Text.Equals(string.Empty))
                    {
                        // MessageBox is shown
                        MessageBox.Show("ERROR: No tags detected. Write some tags.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        // Comments field gets in focus to type
                        likesfromtags_txt.Focus();
                        cant2 = false;
                    }
                    else
                    {
                        tags = likesfromtags_txt.Text.Trim(charsToTrim).Split(',');
                        cant2 = true;
                        // Goes for every tag to add in string
                        foreach (var item in tags)
                        {
                            // If there is empty tag caused with accident comma (ie. " nice, ") just continue
                            if (item.Equals(string.Empty))
                            {
                                continue;
                            }
                            // If there is tag add it in line 
                            else likesFromTagsLine += "'" + item.Trim() + "', ";
                        }
                        likesFromTagsLine = likesFromTagsLine.Remove(likesFromTagsLine.Length - 2, 2) + "], amount=" + likes_nmbr.Value.ToString();

                        //if (likefromtagsphoto.Checked && likefromtagsvideo.Checked || !likefromtagsphoto.Checked && !likefromtagsvideo.Checked)
                        //{
                        // Removes processed comma to add closing bracket
                        likesFromTagsLine += ")" + '\n';
                        //}
                        //else if (likefromtagsphoto.Checked && !likefromtagsvideo.Checked)
                        //{
                        //    likesFromTagsLine += ", media='Photo')" + '\n';
                        //}
                        //else if (!likefromtagsphoto.Checked && likefromtagsvideo.Checked)
                        //{
                        //    likesFromTagsLine += ", media='Video',)" + '\n';
                        //}
                        //if (likesFromTagsLine.StartsWith(","))
                        //{
                        //    likesFromTagsLine = likesFromTagsLine.Remove(likesFromTagsLine.Length - 0, 1);
                        //}
                        File.AppendAllText(FILENAME, likesFromTagsLine);
                    }
                }
                #endregion

                #region Like by Tags and interact with user
                //--
                #endregion

                #region Like by Feeds
                //  This is used to perform likes on your own feeds
                // amount=100  specifies how many total likes you want to perform
                // randomize=True randomly skips posts to be liked on your feed
                // unfollow=True unfollows the author of a post which was considered
                // inappropriate interact=True visits the author's profile page of a
                // certain post and likes a given number of his pictures, then returns to feed
                if (like_by_feed.Checked)
                {
                    bool unfollow = false;
                    if (unfollow_after_like.Checked)
                    {
                        unfollow = true;
                    }
                    string liking = "    session.like_by_feed(amount=" + feed_like_amount.Value.ToString() + ", randomize=False, unfollow=" + unfollow + ", interact=False)" + '\n';
                    File.AppendAllText(FILENAME, liking);
                }
                #endregion

                #region Smart Hashtags
                //# Generate smart hashtags based on https://displaypurposes.com ranking,
                //# banned and spammy tags are filtered out.
                //# (limit) defines amount limit of generated hashtags by hashtag
                //# (sort) sort generated hashtag list 'top' and 'random' are available
                //# (log_tags) shows generated hashtags before use it
                //# (use_smart_hashtags) activates like_by_tag to use smart hashtags
                if (smart_hashtags.Checked)
                {
                    string smarttags = "['";
                    string[] tag = { };
                    if (!title_of_hashtags.Text.Equals(string.Empty))
                    {
                        tag = title_of_hashtags.Text.Trim(charsToTrim).Split(',');

                        foreach (var item in tag)
                        {
                            string item2 = item.Trim();
                            item2 += "\',\'";
                            smarttags += item2.Trim();
                        }
                        smarttags = smarttags.Remove(smarttags.Length - 2, 2) + "]";

                        string smarthashtag = "    session.set_smart_hashtags(" + smarttags + ", limit=" + amount_of_smarttag.Value.ToString() + ", sort='top', log_tags=True)" + '\n';

                        File.AppendAllText(FILENAME, smarthashtag);
                    }
                    else MessageBox.Show("Write down some users.", "ATTENTION", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    string hashtag = "    session.like_by_tags(amount=" + like_per_tag.Value.ToString() + ", use_smart_hashtags=True)" + '\n';
                    if (hashtag.StartsWith(","))
                    {
                        hashtag = hashtag.Remove(hashtag.Length - 0, 1);
                    }
                    File.AppendAllText(FILENAME, hashtag);
                }


                #endregion

                #region Mandatory Words
                //--
                #endregion

                #region Mandatory Language
                //--
                #endregion

                #region Restricting Likes 
                if (restricting_likes.Checked)
                {
                    string tags = "['";
                    string[] tag = { };
                    if (!desired_tag_restriction.Text.Equals(string.Empty))
                    {
                        tag = desired_tag_restriction.Text.Trim(charsToTrim).Split(',');

                        foreach (var item in tag)
                        {
                            string item2 = item.Trim();
                            item2 += "\',\'";
                            tags += item2.Trim();
                        }
                        tags = tags.Remove(tags.Length - 2, 2) + "]";

                        string smarthashtag = "    session.set_dont_like(" + tags + ")" + '\n';
                        if (smarthashtag.StartsWith(","))
                        {
                            smarthashtag = smarthashtag.Remove(smarthashtag.Length - 0, 1);
                        }
                        File.AppendAllText(FILENAME, smarthashtag);
                    }
                    else MessageBox.Show("Write down some users.", "ATTENTION", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                #endregion

                #region Ignoring Restrictions
                //--
                #endregion

                #region Excluding friends
                //--
                #endregion

                #region RUN
                /*========================================================================
				 *			Closing file and running it
				 * 
				 * ========================================================================*/
                if (cant && cant2 && cant3 && cant4 && cant5)
                {
                    File.AppendAllText(FILENAME, '\n' + "    print('------------------')" + '\n'
                                                      + "    print('Finished.')" + '\n'
                                                      + "    while True:" + '\n' + "        " + "time.sleep(10)" + '\n');

                    //File.WriteAllText("Start.bat", "set PYTHONIOENCODING=UTF-8" + '\n' + "py " + FILENAME);
                    //System.Diagnostics.Process.Start("start.bat");
                }


                #endregion

            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void deleteLoginInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void dontShowReadmeInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {


        }

        private void downloadInstaPyToolStripMenuItem_Click(object sender, EventArgs e)
        {


        }

        private void downloadChromedriverToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void likes_nmbr_ValueChanged(object sender, EventArgs e)
        {
            if (likes_nmbr.Value > 1000)
            {
                MessageBox.Show("ATTENTION: Putting amount of likes above 1000 may BAN your account." + '\n' + "We are not responsible if your account get BAN.", "ATTENTION", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void comment_CheckedChanged(object sender, EventArgs e)
        {
            if (comment.Checked)
            {
                panel3.BackColor = System.Drawing.Color.LightGreen;
            }
            else panel3.BackColor = System.Drawing.Color.LightSalmon;
        }

        private void following_CheckedChanged(object sender, EventArgs e)
        {
            if (following.Checked)
            {
                panel4.BackColor = System.Drawing.Color.LightGreen;
            }
            else panel4.BackColor = System.Drawing.Color.LightSalmon;
        }

        private void followfromlist_CheckedChanged(object sender, EventArgs e)
        {
            if (followfromlist.Checked)
            {
                panel12.BackColor = System.Drawing.Color.LightGreen;
            }
            else panel12.BackColor = System.Drawing.Color.LightSalmon;
        }

        private void upperfc_CheckedChanged(object sender, EventArgs e)
        {
            if (upperfc.Checked || lowerfc.Checked)
            {
                panel6.BackColor = System.Drawing.Color.LightGreen;
            }
            else panel6.BackColor = System.Drawing.Color.LightSalmon;
        }

        private void lowerfc_CheckedChanged(object sender, EventArgs e)
        {
            if (lowerfc.Checked || upperfc.Checked)
            {
                panel6.BackColor = System.Drawing.Color.LightGreen;
            }
            else panel6.BackColor = System.Drawing.Color.LightSalmon;
        }

        private void unfollow_CheckedChanged(object sender, EventArgs e)
        {
            if (unfollow.Checked)
            {
                panel7.BackColor = System.Drawing.Color.LightGreen;
            }
            else panel7.BackColor = System.Drawing.Color.LightSalmon;
        }

        private void deleteCredentialsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Login info deleted.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Properties.Settings.Default.usernpass = "";
            Properties.Settings.Default.Save();
        }

        private void downloadInstaPyFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var client = new WebClient())
            {
                client.DownloadFile("https://github.com/timgrossmann/InstaPy/archive/master.zip", "master.zip");
            }



            MessageBox.Show("Extract .zip file and copy InstaPy-GUI.exe and paste it inside InstaPy-master folder.", "ATTENTION", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.Close();
        }

        private void downloadChromedriverToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void deleteCredentialsToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Login info deleted.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Properties.Settings.Default.usernpass = "";
            Properties.Settings.Default.Save();
        }

        private void dontShowReadmeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.readmedont)
            {
                Properties.Settings.Default.readmedont = false;
                dontShowReadmeToolStripMenuItem1.Text = "Do show Readme";
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.readmedont = true;
                dontShowReadmeToolStripMenuItem1.Text = "Don't show Readme";
                Properties.Settings.Default.Save();
            }
        }

        private void downloadInstaPyFilesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (var client = new WebClient())
            {
                client.DownloadFile("https://github.com/timgrossmann/InstaPy/archive/master.zip", "master.zip");
            }
        }

        private void downloadChromedriverToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            using (var client = new WebClient())
            {
                client.DownloadFile("https://chromedriver.storage.googleapis.com/2.29/chromedriver_win32.zip", "chromedriver_win32.zip");
            }


        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            MessageBox.Show("When you find location on Instagram copy only this part of link. Use everything after 'locations/' or just the number" + '\n' + "224442573/salton-sea/" + '\n' + "Without quotation marks!");
            System.Diagnostics.Process.Start("https://www.instagram.com/explore/locations/");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (likefromlocation.Checked)
            {
                panel13.BackColor = System.Drawing.Color.LightGreen;
            }
            else panel13.BackColor = System.Drawing.Color.LightSalmon;

            if (!likefromlocation.Checked)
            {


                likefromtags.Checked = true;

            }
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (FSEF_check.Checked)
            {
                panel14.BackColor = System.Drawing.Color.LightGreen;
            }
            else panel14.BackColor = System.Drawing.Color.LightSalmon;
        }

        private void fusef_ch_CheckedChanged(object sender, EventArgs e)
        {
            if (fusef_ch.Checked)
            {
                panel15.BackColor = System.Drawing.Color.LightGreen;
            }
            else panel15.BackColor = System.Drawing.Color.LightSalmon;
        }

        private void checkBox1_CheckedChanged_2(object sender, EventArgs e)
        {
            if (fi_ch.Checked)
            {
                panel16.BackColor = System.Drawing.Color.LightGreen;
            }
            else panel16.BackColor = System.Drawing.Color.LightSalmon;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged_3(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Numberofphotos_Click(object sender, EventArgs e)
        {

        }

        private void Numberoftags_ValueChanged(object sender, EventArgs e)
        {

        }

        private void panel17_Paint(object sender, PaintEventArgs e)
        {
            if (FollowByTags.Checked)
            {
                panel17.BackColor = System.Drawing.Color.LightGreen;
            }
            else panel17.BackColor = System.Drawing.Color.LightSalmon;
        }

        private void panel18_Paint(object sender, PaintEventArgs e)
        {
            if (Followthelikersofphotosofusers.Checked)
            {
                panel18.BackColor = System.Drawing.Color.LightGreen;
            }
            else panel18.BackColor = System.Drawing.Color.LightSalmon;
        }

        private void label28_Click(object sender, EventArgs e)
        {

        }

        private void panel20_Paint(object sender, PaintEventArgs e)
        {
            if (Interact_with_users.Checked)
            {
                panel20.BackColor = System.Drawing.Color.LightGreen;
            }
            else panel20.BackColor = System.Drawing.Color.LightSalmon;
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void location_finder_Click(object sender, EventArgs e)
        {
            MessageBox.Show("When you find location on Instagram copy only this part of link. Use everything after 'locations/' or just the number" + '\n' + "224442573/salton-sea/" + '\n' + "Without quotation marks!");
            System.Diagnostics.Process.Start("https://www.instagram.com/explore/locations/");
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void unfollow_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void label36_Click(object sender, EventArgs e)
        {

        }

        private void unfollow_check_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void panel23_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
