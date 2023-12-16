using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;

namespace ClassLibraryTehtavaOjalehto
{
    public class DataHandling : db
{
        private DataTable dt = new DataTable();

        //muuttuja ex varten
        private string serr = "0";
        private int found = 0;

        //Tämä tässä on property declaration
        public DataTable Dt
        {
            get
            {
                return dt;
            }
            set
            {
                dt = value;
            }
        }


        public string sErr
        {
            get
            {
                return serr;
            }
            set
            {
                serr = value;
            }
        }

        public int Found
        {
            get
            {
                return found;
            }
            set
            {
                found = value;
            }
        }

        //Getter pelaajatiedoille
        public void getPlayers()
        {

            try
            {
                //Luodaan muuttuja ja olio
                //SQLiteConnection sqlite_conn;
                //sqlite_conn = new SQLiteConnection("Data source = customers.db; Version=3; New=False; Compression = True");
                //Tietokannan avaus -> Toimii nyt inheritance kautta
                sqlite_conn.Open();
                //Luodaan muuttujia ja olioita
                SQLiteDataReader sqlite_datareader;
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = sqlite_conn.CreateCommand();
                //Haetaan tietoja
                sqlite_cmd.CommandText = "SELECT * FROM player ORDER BY ID";
                sqlite_datareader = sqlite_cmd.ExecuteReader();

                Dt.Load(sqlite_datareader);

                //Suljetaan tietokanta
                sqlite_conn.Close();
            }
            catch (Exception ex)
            {

                this.sErr = ex.ToString();
            }
        }

        //Getter yksittäiselle pelaajalle
        public void getPlayer(string sID)
        {
            try
            {
                int iId = Int32.Parse(sID);

                sqlite_conn.Open();
                //Luodaan muuttujia ja olioita
                SQLiteDataReader sqlite_datareader;
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = sqlite_conn.CreateCommand();
                //Haetaan tietoja                                                   //Muuttuja lisätään loppuun
                sqlite_cmd.CommandText = "SELECT * FROM player WHERE ID = " + iId;
                sqlite_datareader = sqlite_cmd.ExecuteReader();

                Dt.Load(sqlite_datareader);

                //Suljetaan tietokanta
                sqlite_conn.Close();
            }
            catch (Exception ex)
            {

                this.sErr = ex.ToString();
            }
        }
        //Getter toiselle taululle db.ssä
        public void getScores()
        {
            try
            {
                sqlite_conn.Open();
                SQLiteDataReader sqlite_datareader;
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = "SELECT * FROM score ORDER BY playersID";
                sqlite_datareader = sqlite_cmd.ExecuteReader();

                Dt.Load(sqlite_datareader);

                sqlite_conn.Close();
            }
            catch (Exception ex)
            {
                this.sErr = ex.ToString();
            }
        }

        public void addPlayer(string sName, string sEmail, string sPassword)
        {
            try
            {
                sqlite_conn.Open();
                //Luodaan muuttujia ja olioita
                SQLiteDataReader sqlite_datareader;
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = sqlite_conn.CreateCommand();
                //Katsotaan onko sisältöä                                              //Muuttuja lisätään loppuun
                sqlite_cmd.CommandText = "SELECT * FROM player WHERE email = '" + sEmail + "'";
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                
                if (sqlite_datareader.HasRows)
                {
                    this.Found = 1;
                    //Jos löytyy datareader kiinni.
                    sqlite_datareader.Close();
                    sqlite_conn.Close();
                    return;
                }
                else
                {
                    //Lisätään uusi asiakas jos kohta on tyhjä.
                    sqlite_cmd = sqlite_conn.CreateCommand();
                    sqlite_cmd.CommandText = "INSERT INTO player (name, email, password) VALUES('" + sName + "', '" + sEmail + "', '" + sPassword + "')";
                    sqlite_cmd.ExecuteNonQuery();

                  

                    sqlite_conn.Close();


                }
            }
            catch (Exception ex)
            {
                this.sErr = ex.ToString();
            }
        }
        public void addScore(string sDate, int iScore,int playersID)
        {

           

            try
            {
                sqlite_conn.Open();

                //Tarkistetaan onko tälle päivälle jo samalla pelaajalla pisteet.
                SQLiteCommand checkScoreCmd = sqlite_conn.CreateCommand();
                checkScoreCmd.CommandText = "SELECT COUNT(*) FROM score WHERE date = '" + sDate + "' AND playersID = " + playersID;
                int scoreCount = Convert.ToInt32(checkScoreCmd.ExecuteScalar());

                if (scoreCount > 0)
                {
                   //Jos on jo olemassa
                    this.Found = 1;
                    sqlite_conn.Close();
                    return;
                }

                // Asetetaan uudet pisteet
                SQLiteCommand insertScoreCmd = sqlite_conn.CreateCommand();
                insertScoreCmd.CommandText = "INSERT INTO Score (date, score, playersID) VALUES('" + sDate + "', " + iScore + ", " + playersID + ")";
                insertScoreCmd.ExecuteNonQuery();

                sqlite_conn.Close();
            }
            catch (Exception ex)
            {
                this.sErr = ex.ToString();
            }


        }

        public string getPlayerName(int playerId)
        {
            try
            {
                
                sqlite_conn.Open();

                // Haetaan pelajaan nimi ID perusteella
                SQLiteCommand cmd = sqlite_conn.CreateCommand();
                cmd.CommandText = "SELECT name FROM player WHERE ID = " + playerId;

                // suoritetaan
                object tulos = cmd.ExecuteScalar();

                
                sqlite_conn.Close();

                // Tarkisteteaan löytyikö.
                if (tulos != null)
                    return tulos.ToString();
                else
                    return "Pelaajaa ei löytynyt";
            }
            catch (Exception ex)
            {
                
                sErr = ex.ToString();
                return "Jokin meni vikaan";
            }
        }

        public void updatePlayer(int iID, string sName, string sEmail, string sPassword)
        {
            try
            {
                //int iId = Int32.Parse(sID);

                sqlite_conn.Open();
                //Luodaan muuttujia ja olioita
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = sqlite_conn.CreateCommand();
                //Lisätään tietoja                                                 
                sqlite_cmd.CommandText = "UPDATE player SET name = '" + sName + "', email = '" + sEmail + "', password = '" + sPassword + "' WHERE ID = " + iID;
                //Käsketään suorittaa
                sqlite_cmd.ExecuteNonQuery();



                //Suljetaan tietokanta
                sqlite_conn.Close();
            }
            catch (Exception ex)
            {

                this.sErr = ex.ToString();
            }



        }


        public void updateScore(string sDate, int iScore, int playersID)
        {
            try
            {
                

                sqlite_conn.Open();
                //Luodaan muuttujia ja olioita
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = sqlite_conn.CreateCommand();
                //Lisätään tietoja                                                 
                sqlite_cmd.CommandText = "UPDATE score SET date = '" + sDate + "', score = '" + iScore + "' WHERE playersID = " + playersID;
                //Käsketään suorittaa
                sqlite_cmd.ExecuteNonQuery();



                //Suljetaan tietokanta
                sqlite_conn.Close();
            }
            catch (Exception ex)
            {

                this.sErr = ex.ToString();
            }



        }

        public void removePlayer(int iId)
        {
            try
            {
                

                sqlite_conn.Open();
                //Luodaan muuttujia ja olioita
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = sqlite_conn.CreateCommand();
                //Lisätään tietoja                                                 
                sqlite_cmd.CommandText = "DELETE FROM player WHERE ID = " + iId + "";
                //Käsketään suorittaa
                sqlite_cmd.ExecuteNonQuery();

                //Deletoidaan myös score taulun tiedot
                sqlite_cmd.CommandText = "DELETE FROM score WHERE playersID = " + iId;
                sqlite_cmd.ExecuteNonQuery();

                //Suljetaan tietokanta
                sqlite_conn.Close();
            }
            catch (Exception ex)
            {

                this.sErr = ex.ToString();
            }


        }

    }
    }

