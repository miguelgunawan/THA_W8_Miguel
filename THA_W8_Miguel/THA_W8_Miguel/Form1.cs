using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace THA_W8_Miguel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string koneksi = "server=localhost;uid=root;pwd=;database=premier_league";
        MySqlConnection sqlConnection;
        MySqlCommand sqlCommand;
        MySqlDataAdapter sqldataadapter;

        string team = "";
        string player = "";
        string player2 = "";
        string profilplayer1 = "";
        string profilplayer2 = "";
        string info = "";
        string detailmatches = "";
        string teamhome = "";
        string teamaway = "";

        DataTable dtteam = new DataTable();
        DataTable dtplayer = new DataTable();
        DataTable dtplayer2 = new DataTable();
        DataTable dtprofilplayer1 = new DataTable();
        DataTable dtprofilplayer2 = new DataTable();
        DataTable dtinfo = new DataTable();
        DataTable datadetail = new DataTable();
        DataTable datatimhome = new DataTable();
        DataTable datatimaway = new DataTable();

        private void Form1_Load(object sender, EventArgs e)
        {
          
        }

        private void playerDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dtteam.Clear();
            dtplayer.Clear();
            panel1.Visible = true;
            panel2.Visible = true;
            panel3.Visible = false;

            team = "SELECT team_id as 'ID',team_name as 'Team Name' from team";
            sqlConnection = new MySqlConnection(koneksi);
            sqlCommand = new MySqlCommand(team,sqlConnection);
            sqldataadapter = new MySqlDataAdapter(sqlCommand);

            sqldataadapter.Fill(dtteam);
            comboBox1.DataSource = dtteam;
            comboBox1.ValueMember = "ID";
            comboBox1.DisplayMember = "Team Name";
            comboBox2.Text = "";
            comboBox1.Text = "";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
          

        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            comboBox2.Text = "";

            player = "SELECT p.player_name as 'name',p.player_id as 'IDplayer' from player p,team t where p.team_id=t.team_id and t.team_id=" + $"'{comboBox1.SelectedValue.ToString()}';";
            sqlCommand = new MySqlCommand(player, sqlConnection);
            sqldataadapter = new MySqlDataAdapter(sqlCommand);
            sqldataadapter.Fill(dtplayer);

            comboBox2.DataSource = dtplayer;
            comboBox2.ValueMember = "IDplayer";
            comboBox2.DisplayMember = "name";
            comboBox2.Text = "";
        }

        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            dtprofilplayer2.Clear();
            dtprofilplayer1.Clear();
            profilplayer1 = "select p.player_name as 'player name',t.team_name as 'team name',n.nation as 'nationality',p.team_number as 'pemain number' from player p ,team t,nationality n where t.team_id=p.team_id and p.nationality_id=n.nationality_id and p.player_id =" + $"'{comboBox2.SelectedValue.ToString()}';";
            sqlCommand = new MySqlCommand(profilplayer1, sqlConnection);
            sqldataadapter = new MySqlDataAdapter(sqlCommand);
            sqldataadapter.Fill(dtprofilplayer1);

            profilplayer2 = "select COALESCE(sum(if(d.type='CY',1,0)),0) as 'Yellow Card',COALESCE(sum(if(d.type='CR',1,0)),0) as'Red Card',COALESCE(sum(if(d.type='GO',1,0)),0) as 'goal',COALESCE(sum(if(d.type='GW',1,0)),0) as 'Own Goal' ,COALESCE(sum(if(d.type='GP',1,0)),0) as 'Goal Penalty',COALESCE(sum(if(d.type='PM',1,0)),0) as 'pm'from dmatch d,player p where p.player_id = d.player_id and p.player_id=" + $"'{comboBox2.SelectedValue.ToString()}';";
            sqlCommand = new MySqlCommand(profilplayer2, sqlConnection);
            sqldataadapter = new MySqlDataAdapter(sqlCommand);
            sqldataadapter.Fill(dtprofilplayer2);

            label10.Text = dtprofilplayer1.Rows[0][1].ToString();
            label11.Text = dtprofilplayer1.Rows[0][0].ToString();
            label13.Text = dtprofilplayer1.Rows[0][2].ToString();
            label14.Text = dtprofilplayer1.Rows[0][3].ToString();

            label15.Text = dtprofilplayer2.Rows[0][0].ToString();
            label16.Text = dtprofilplayer2.Rows[0][1].ToString();
            label17.Text = dtprofilplayer2.Rows[0][2].ToString();
            label18.Text = dtprofilplayer2.Rows[0][3].ToString();
        }

        private void findMatchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = true;
            comboBox3.Visible = true;
            comboBox4.Visible = true;
            dataGridView1.Visible = true;
            dataGridView2.Visible = true;
            dataGridView3.Visible = true;

            player2 = "SELECT team_id as 'ID',team_name as 'Team Name' from team";
            sqlConnection = new MySqlConnection(koneksi);
            sqlCommand = new MySqlCommand(player2, sqlConnection);
            sqldataadapter = new MySqlDataAdapter(sqlCommand);
            sqldataadapter.Fill(dtplayer2);

            comboBox3.DataSource = dtplayer2;
            comboBox3.ValueMember = "ID";
            comboBox3.DisplayMember = "Team Name";
            comboBox3.Text = "";
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox4_SelectionChangeCommitted(object sender, EventArgs e)
        {
            datadetail.Clear();
            datatimhome.Clear();
            datatimaway.Clear();

            //detail matchnya
            detailmatches = "SELECT d.`minute` as'minute',t.team_name as'Team Name',p.player_name as 'PlayerName',if(d.type='CY','Yellow Card',if(d.type='CR','Red Card',if(d.type='GO','Goal',if(d.type='GW','OwnGoal',if(d.type='Gp','Goal Penalty','Penalty Miss')))))from dmatch d,team t,player p where d.team_id=t.team_id and d.player_id=p.player_id and d.match_id=" + $"'{comboBox4.SelectedValue.ToString()}';";
            sqlConnection = new MySqlConnection(koneksi);
            sqlCommand = new MySqlCommand(detailmatches, sqlConnection);
            sqldataadapter = new MySqlDataAdapter(sqlCommand);
            sqldataadapter.Fill(datadetail);
            dataGridView1.DataSource = datadetail;
            //temahome
            teamhome = $"SELECT t.team_name as'Home Team',p.player_name as 'Home Player',p.playing_pos as'Position'from `match`m,team t,player p where p.team_id=t.team_id and t.team_id=m.team_home and m.match_id='{comboBox4.SelectedValue.ToString()}';";
            sqlConnection = new MySqlConnection(koneksi);
            sqlCommand = new MySqlCommand(teamhome, sqlConnection);
            sqldataadapter = new MySqlDataAdapter(sqlCommand);
            sqldataadapter.Fill(datatimhome);
            dataGridView2.DataSource = datatimhome;
            //teamaway
            teamaway = $"SELECT t.team_name as'Away Team',p.player_name as 'Away Player',p.playing_pos as'Position'from `match`m,team t,player p where p.team_id=t.team_id and t.team_id=m.team_away and m.match_id='{comboBox4.SelectedValue.ToString()}';";
            sqlConnection = new MySqlConnection(koneksi);
            sqlCommand = new MySqlCommand(teamaway, sqlConnection);
            sqldataadapter = new MySqlDataAdapter(sqlCommand);
            sqldataadapter.Fill(datatimaway);
            dataGridView3.DataSource = datatimaway;
        }

        private void comboBox3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            dtinfo.Clear();
            info = $"SELECT t.team_id as'Team Id',m.match_id as'ID',m.match_date as'Match Date',t.team_name'team home',t2.team_name as 'Team Away'from dmatch d,`match` m, team t ,team t2 where d.match_id=m.match_id and t.team_id=m.team_home and t2.team_id = m.team_away and (t.team_id = '{comboBox3.SelectedValue.ToString()}'or t2.team_id = '{comboBox3.SelectedValue.ToString()}') group by d.match_id;";
            sqlConnection = new MySqlConnection(koneksi);
            sqlCommand = new MySqlCommand(info, sqlConnection);
            sqldataadapter = new MySqlDataAdapter(sqlCommand);
            sqldataadapter.Fill(dtinfo);

            comboBox4.DataSource = dtinfo;
            comboBox4.ValueMember = "ID";
            comboBox4.DisplayMember = "ID";
            comboBox4.Text = "";
        }
    }
}
