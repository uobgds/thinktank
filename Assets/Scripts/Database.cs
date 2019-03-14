using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

namespace ExampleProject
{

    public class Database : MonoBehaviour
    {

        private string dbPath;

        public void Start()
        {
            dbPath = "URI=file:" + Application.persistentDataPath + "/exampleDatabase.db";
            CreateSchema();           
        }

        public void CreateSchema()
        {
            using (var conn = new SqliteConnection(dbPath))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS 'high_score' ( " +
                                      "  'id' INTEGER PRIMARY KEY, " +
                                      "  'name' TEXT NOT NULL, " +
                                      "  'score' INTEGER NOT NULL, " +
                                      "  'difficulty' TEXT NOT NULL " +
                                      ");";

                    var result = cmd.ExecuteNonQuery();
                    Debug.Log("create schema: " + result);
                }
            }
        }

        public void InsertScore(string highScoreName, int score)
        {
            using (var conn = new SqliteConnection(dbPath))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO high_score (name, score, difficulty) " +
                                      "VALUES (@Name, @Score, @Difficulty);";

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "Name",
                        Value = highScoreName
                    });

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "Score",
                        Value = score
                    });

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "Difficulty",
                        Value = GameSettings.difficulty.ToString()
                    });

                    var result = cmd.ExecuteNonQuery();
                    Debug.Log("insert score: " + result);

                }
            }
        }

        public ArrayList GetHighScores(int limit)
        {
            ArrayList temp = new ArrayList();
            using (var conn = new SqliteConnection(dbPath))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM high_score WHERE difficulty='" + GameSettings.difficulty.ToString() + "' ORDER BY score DESC LIMIT @Count;";

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "Count",
                        Value = limit
                    });

                    Debug.Log("scores (begin)");
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        var highScoreName = reader.GetString(1);
                        var score = reader.GetInt32(2);
                        var difficulty = reader.GetString(3);

                        var text = string.Format("{0}: {1} [#{2}] {3}", highScoreName, score, id, difficulty);
                        temp.Add(text);
                        Debug.Log(text);
                    }
                    Debug.Log("scores (end)");

                }
            }
            return temp;
        }

        public void ClearDatabase()
        {
            using (var conn = new SqliteConnection(dbPath))
            {
                conn.Open();
                using(var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "DELETE from high_score";
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }

}