using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ExpertSystemProject_Main
{
 
    public class Node_collection
    {
        [XmlArray("Nodes"), XmlArrayItem("Node")]
        public List<Node> nodes { get; set; }

        public Node_collection()
        {
            
        }
        
    }
    public class Node
    {
        [XmlIgnore]
        static int counter = 1;

        public static void reset_counter()
        {
            counter = 1;
        }

        [XmlElement("id")]
        public int id;

        [XmlElement("qustion_id")]
        public int question_id;


        //public List<Edge> edges;

        [XmlElement("nodes_edges")]
        public Edge_collection edges_collection;


        public Node()
        {
            id = 0;
            question_id = 0;
            edges_collection = new Edge_collection();
            //edges_collection.edges = new List<Edge>();

        }
        public Node(int question_ID)
        {
            edges_collection = new Edge_collection();
            question_id = question_ID;
            edges_collection.edges = new List<Edge>();
            id = counter;
            counter++;
        }
        public Node(int question_ID,int ID)
        {
            edges_collection = new Edge_collection();
            question_id = question_ID;
            edges_collection.edges = new List<Edge>();
            id = ID;
           
        }


        public List<Edge> get_edges()
        {
            return edges_collection.edges;
        }

        //update before saving
        public void  save_update_edges_collection()
        {
           // edges_collection.edges = this.edges;
        }
        public void load_update_edges_collection()
        {
            //this.edges = edges_collection.edges;
        }
    }

    public class Edge_collection
    {
        [XmlArray("Edges"), XmlArrayItem("Edge")]
        public List<Edge> edges { get; set; }

        public Edge_collection()
        {
            edges = new List<Edge>();
        }
    }
    public class Edge
    {
        [XmlElement("fact_id")]
        public int fact_id;
        [XmlElement("node_id")]
        public int node_id;

        public Edge()
        {
            fact_id = 0;
            node_id = 0;
        }
        public Edge(int fact_ID, int node_ID)
        {
            fact_id = fact_ID;
            node_id = node_ID;

        } 
    }

    public class Question_collection
    {
        [XmlArray("Qustions"), XmlArrayItem("Qustion")]
        public List<Question> qustions { get; set; }
    }
    public class Question
    {
        [XmlIgnore,]
        static int counter = 1;

        public static void reset_counter()
        {
            counter = 1;
        }

        [XmlElement("text")]
        public string text;
        [XmlElement("multimedia_name")]
        public string multimedia_name;
        [XmlElement("id")]
        public int id;

        public Question()
        {

        }
        public Question(string Text, string multimedia_Name)
        {
            text = Text;
            multimedia_name = multimedia_Name;
            id = counter;

            counter++;
        }
        public Question(string Text)
        {
            text = Text;
            multimedia_name = null;
            id = counter;
            counter++;
        }
        public Question(string Text, int ID)
        {
            text = Text;
            multimedia_name = null;
            id = ID;
        }
    }

    public class Fact_collection
    {
        [XmlArray("Facts"), XmlArrayItem("Fact")]
        public List<Fact> facts { get; set; }


    }
    public class Fact
    {
        [XmlIgnore]
        static int counter = 1;

        public static void reset_counter()
        {
            counter = 1;
        }

        [XmlElement("id")]
        public int id;
        [XmlElement("text")]
        public string text;
        [XmlElement("multimedia_name")]
        public string multimedia_name;

        public Fact()
        {

        }
        public Fact(string Text,int ID)
        {
            text = Text;
            id = ID;
        }
        public Fact(string Text,string MultimediaName)
        {
            text = Text;
            multimedia_name = MultimediaName;
            id = counter;

            counter++;
        }
        public Fact(string Text)
        {
            text = Text;
            multimedia_name = null;
            id = counter;
            counter++;
        }
    }

   
}

