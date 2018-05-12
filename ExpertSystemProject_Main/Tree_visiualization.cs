using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;



namespace ExpertSystemProject_Main
{
    class Node_view
    {

        public static Size size = new Size(100, 40);
        public static Size ellipse_size = new Size(size.Height / 2, size.Height / 2);
        static Font font = new Font("Arial", 7, FontStyle.Regular);
        static Color rect_default_color = Color.FloralWhite;
        static Color rect_highlighted_color = Color.LightGreen;
        static Color ellipse_color = Color.Red;

        
        public Rectangle ellipse;
        public Rectangle rect;
        public string question_text;
        Question question;
        public string image_name;
        public Node node;
        public Node_view parent_node;

        public List<Edge_view> edge_views;
        public Edge_view parent_edge;

        public Node_view(Point Location, Node Node, List<Question> quest)
        {
            rect = new Rectangle(Location, size);

            Point ellipse_location = new Point(rect.X + rect.Width - ellipse_size.Width / 2, rect.Y + rect.Height/2 - ellipse_size.Height / 2);
            ellipse = new Rectangle(ellipse_location, ellipse_size);

            node = Node;

            Question qu = quest.Find(x => x.id == node.question_id);
            question_text = qu.text;
            image_name = qu.multimedia_name;

           
            edge_views = new List<Edge_view>();



        }
        public Node_view(Point Location)
        {
            rect = new Rectangle(Location, size);

            Point ellipse_location = new Point(rect.X + rect.Width - ellipse_size.Width / 2, rect.Y + rect.Height / 2 - ellipse_size.Height / 2);
            ellipse = new Rectangle(ellipse_location, ellipse_size);

            question_text = "new qustion";
            image_name = null;

            edge_views = new List<Edge_view>();

        }

        public void update_elipse()
        {
            Point ellipse_location = new Point(rect.X + rect.Width - ellipse_size.Width / 2, rect.Y + rect.Height / 2 - ellipse_size.Height / 2);
            ellipse = new Rectangle(ellipse_location, ellipse_size);
        }
        public void update_edges()
        {
            foreach(Edge_view ev in edge_views)
            {
                ev.update_points();
            }
        }

        public void draw(Graphics e)
        {
            Pen pen = new Pen(Color.Black);
            SolidBrush default_brush = new SolidBrush(rect_default_color);
            SolidBrush font_brush = new SolidBrush(Color.Black);


            e.FillRectangle(default_brush, rect);
            e.DrawString(question_text, font, font_brush, rect);
            e.DrawRectangle(pen, rect);

        }
        public void draw_highlited(Graphics e)
        {
            Pen pen = new Pen(Color.Black);
            SolidBrush default_brush = new SolidBrush(rect_default_color);
            SolidBrush highlited_brush = new SolidBrush(rect_highlighted_color);
            SolidBrush ellipse_brush = new SolidBrush(ellipse_color);

            SolidBrush font_brush = new SolidBrush(Color.Black);


            e.FillRectangle(highlited_brush, rect);
            e.DrawString(question_text, font, font_brush, rect);
            e.DrawRectangle(pen, rect);
            e.FillEllipse(ellipse_brush, ellipse);            
        }

        public void move(Point P)
        {
            P.X = P.X - rect.Width / 2;
            P.Y = P.Y - rect.Height / 2;
            rect.Location = P;
            update_elipse();
            update_edges();
            if(parent_edge!= null) parent_edge.update_points();


        }

        public bool isClickedOnRectangle(Point clickPoint)
        {
            bool is_in_width = (clickPoint.X > rect.X) && (clickPoint.X < rect.X+rect.Width);
            bool is_in_height = (clickPoint.Y > rect.Y) && (clickPoint.Y < rect.Y + rect.Height);

            return (is_in_height && is_in_width);
        }
        public bool isClickedOnEllipse(Point clickPoint)
        {
            bool is_in_ellipse = ((clickPoint.X - ellipse.X+ ellipse.Height / 2) ^ 2 + (clickPoint.Y - ellipse.Y+ ellipse.Height / 2) ^ 2) < ((ellipse.Height / 2)^2);
            return is_in_ellipse;
        }

        public Question get_qustion()
        {
            question = new Question(question_text, image_name);
            return question;
        }
        public Node init_node()
        {
            node = new Node(question.id);          
            return node;
        }
        public List<Edge> init_edges()
        {
            List<Edge> edges = new List<Edge>();
            foreach(Edge_view ev in edge_views)
            {
                edges.Add(ev.get_edge());
            }

            node.edges_collection.edges = edges;
            return edges;
        }
        public Node get_node()
        {
            return node;
        }

    }

    class Edge_view
    {
        static Color edge_color = Color.LightGray;
        static Color heighlited_color = Color.LightGreen;

        public Edge edge;
        public Fact fact;
        public string text;
        Rectangle text_rect;
        Point start;
        Point end;

        public Node_view source_node;
        public Node_view destination_node;

        public Edge_view(Node_view Source,Node_view Destination,Edge ed, List<Fact> facts)
        {
            edge = ed;
            source_node = Source;
            destination_node = Destination;

            update_points();
            fact = new Fact();

            fact = facts.Find(x => x.id == edge.fact_id);
            text = fact.text;
        }
        public Edge_view(Node_view Source, Node_view Destination)
        {
            source_node = Source;
            destination_node = Destination;

            update_points();
            fact = new Fact();

            text = "new answer";
        }
        public void update_points()
        {
            start = new Point(source_node.rect.X + source_node.rect.Width, source_node.rect.Y + source_node.rect.Height / 2);
            end = new Point(destination_node.rect.X, destination_node.rect.Y + destination_node.rect.Height / 2);

            text_rect = new Rectangle((end.X-start.X)/3 + start.X-5, (end.Y- start.Y) / 3 + start.Y+5, 63, 30);

        }

        public void draw(Graphics e)
        {
            Pen pen = new Pen(edge_color, 3);
            pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            Font font = new Font("Arial", 7, FontStyle.Regular);
            SolidBrush font_brush = new SolidBrush(Color.Black);

            e.DrawLine(pen, start, end);
          //  e.FillRectangle(new SolidBrush(Color.White), text_rect);
            e.DrawString(text, font, font_brush, text_rect);
           // e.DrawRectangle(new Pen(Color.Black), text_rect);
        }
        public void draw_heighlited(Graphics e)
        {
            Pen pen = new Pen(Color.Red,3);
            pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            SolidBrush brush = new SolidBrush(heighlited_color);

            Font font = new Font("Arial", 7, FontStyle.Regular);
            SolidBrush font_brush = new SolidBrush(Color.Black);

            e.DrawLine(pen, start, end);
            e.FillRectangle(brush, text_rect);
            e.DrawString(text, font, font_brush, text_rect);
            e.DrawRectangle(new Pen(Color.Black), text_rect);
        }

        public bool isClickedOnEdge(Point clickPoint)
        {
            bool is_in_width = (clickPoint.X > text_rect.X) && (clickPoint.X < text_rect.X + text_rect.Width);
            bool is_in_height = (clickPoint.Y > text_rect.Y) && (clickPoint.Y < text_rect.Y + text_rect.Height);

            return (is_in_height && is_in_width);
        }

        public void remove_self()
        {
            source_node.edge_views.Remove(this);
        }

        public Edge get_edge()
        {
            
            edge = new Edge(fact.id, destination_node.node.id);
            return edge;
        }
        public Fact get_fact()
        {
            fact = new Fact(text);
            return fact;
        }
    }

}
