using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace ExpertSystemProject_Main
{
    public partial class Form1 : Form
    {
        Size default_size;

        public Node current_node;
        public Node start_node;

        public List<Question> questions; //TODO: вместо списков словарь key -- id
        public List<Fact> facts;
        public List<Node> nodes;
        
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {   
            
            this.Text = "Экспертная система";

            

            default_size = this.Size;

            init_save_module();

            
            //  load_expert_system(); Убрал Радмир
         
            // init_work_space(); Убрал Радмир
            // close_work_space(); Убрал Радмир

            init_main_menu();
            // start_node = nodes[0]; Убрал Радмир
            // current_node = start_node; Убрал Радмир
            // init_graphical_editor(); Убрал Радмир
            // close_graphic_editor(); Убрал Радмир
        }


        //------main menu----------
        List<Control> main_menu_controls;
        Label title; 
        Label Current_ES; //Добавил Радмир
        Button chose_es; //Добавил Радмир
        String PathOfLoad; //Добавил Радмир
        String PathOfSave; //Добавил Радмир
        Button start;
        Button text_edit;
        Button graphic_edit;

        void init_main_menu()
        {
            init_title();
            init_Current_ES();
            init_start_button();
            init_text_edit_button();
            init_graphic_edit_button();
            init_chose_es_button();


            this.Controls.Add(Current_ES);
            this.Controls.Add(title);
            this.Controls.Add(start);
            this.Controls.Add(chose_es);
           // this.Controls.Add(text_edit);
            this.Controls.Add(graphic_edit);

            main_menu_controls = new List<Control>(6);
            main_menu_controls.Add(title);
            main_menu_controls.Add(start);
            main_menu_controls.Add(text_edit);
            main_menu_controls.Add(graphic_edit);
            main_menu_controls.Add(chose_es);
            main_menu_controls.Add(Current_ES);
        }
        

        void init_title()
        {
            title = new Label();
            title.AutoSize = true;
            title.Text = "Current Expert System:";
            title.Location = new Point(100, 100);
           
        }

        void init_Current_ES()
        {
            Current_ES = new Label();
            Current_ES.AutoSize = true;
            Current_ES.Text = "NONE";
            Current_ES.Location = new Point(210, 100);


        }
        void init_start_button()
        {
            start = new Button();
            start.Text = "Start";
            start.Location = new Point(230, 200);
            start.Click += new EventHandler(start_button_click);

        }
        // Добавил Радмир --------------------------------------------------------
        void init_chose_es_button()
        {
            chose_es = new Button();
            chose_es.Text = "Chose ES";
            chose_es.Location = new Point(230, 150);
            chose_es.Click += new EventHandler(chose_es_button_click);


        }
        //----------------------------------------------------------------------------
        void init_text_edit_button()
        {
            text_edit = new Button();
            text_edit.Text = "text editor";
            text_edit.Location = new Point(230, 230);
            text_edit.Click += new EventHandler(text_edit_button_click);
        }
        void init_graphic_edit_button()
        {
            graphic_edit = new Button();
            graphic_edit.Text = "graphic editor";
            graphic_edit.AutoSizeMode = AutoSizeMode.GrowOnly;
            graphic_edit.AutoSize = true;
            graphic_edit.Location = new Point(230, 230);
            graphic_edit.Click += new EventHandler(graphic_edit_button_click);
        }

        // Добавил Радмир -------------------------------------------------------
        void chose_es_button_click(object sender,EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            fd.SelectedPath = Environment.CurrentDirectory + '\\' + "Expert Systems";
            fd.ShowDialog();
            PathOfLoad = fd.SelectedPath;
            string[] st = PathOfLoad.Split('\\');
            Current_ES.Text = st[st.Length - 1];
        }
        //--------------------------------------------------------------------------
        void start_button_click(object sender, EventArgs e)
        {
            load_expert_system(); // Добавил Радмир
            init_work_space();// Добавил Радмир
            close_work_space();// Добавил Радмир
            close_main_menu();
            open_work_space();
            

        }
        void text_edit_button_click(object sender, EventArgs e)
        {
            Button bt = sender as Button;

            if (bt == null)
            {
                
                return;
            }



        }
        void graphic_edit_button_click(object sender, EventArgs e)
        {  
            //start_node = nodes[0];// Добавил Радмир
            //current_node = start_node; //Добавил Радмир

            init_graphical_editor(); //Добавил Радмир
             close_graphic_editor(); //Добавил Радмир
         
            close_main_menu();
            open_graphic_editor();

        }

        void close_main_menu()
        {
            foreach(Control control in main_menu_controls)
            {
                control.Visible = false;
            }
        }
        void open_main_menu()
        {
            foreach (Control control in main_menu_controls)
            {
                control.Visible = true;
            }
        }

        //----graphical editor-----
        List<Control> editor_controls;
        PictureBox editor_workspace;
        Panel panel;
        MenuStrip editor_menu;
        FlowLayoutPanel node_edit_panel;
        FlowLayoutPanel edge_edit_panel;
        TextBox productions_print;

        List<Node_view> node_views;
        List<Edge_view> edge_views;

        Node_view current_node_view;
        Edge_view current_edge_view;
        Point startPoint;
        int offset_X;
        int offset_Y;
        int width;

        void init_graphical_editor()
        {
            init_editor_workspace();
            //this.Controls.Add(editor_workspace);

            init_node_edit_panel();
            this.Controls.Add(node_edit_panel);

            init_edge_edit_panel();
            this.Controls.Add(edge_edit_panel);

            init_editor_menu();
            this.Controls.Add(editor_menu);

            init_panel();
            this.Controls.Add(panel);

            init_productions_print();
            this.Controls.Add(productions_print);
            productions_print.Visible = true;

            editor_controls = new List<Control>();
            add_editor_controls_to_list();

            

            startPoint = new Point(10, 10);
            offset_X = (int)(Node_view.size.Width * 2.1);
            offset_Y = (int)(Node_view.size.Height * 1.5);

            node_views = new List<Node_view>();
            edge_views = new List<Edge_view>();

            width = 0;
            isCreationEdge = false;
            isCreationNode = false;
            isMovingNode = false;

         //   create_node_views_list(); Убрал Радмир
          //  create_edge_views_list(); Убрал Радмир

            change_edit_pannels();
        }
        void init_editor_workspace()
        {
            
            editor_workspace = new PictureBox();
            editor_workspace.Location = new Point(0, 0);
            editor_workspace.Size = new Size(10000, 6850);
            editor_workspace.BackColor = Color.SeaShell;
            editor_workspace.BorderStyle = BorderStyle.Fixed3D;
            editor_workspace.SizeMode = PictureBoxSizeMode.AutoSize;

            editor_workspace.Paint += new PaintEventHandler(pictureBox_Paint);
            editor_workspace.MouseClick += new MouseEventHandler(pictureBox_mouseClick);
            editor_workspace.MouseDown += new MouseEventHandler(pictureBox_mouseDown);
            editor_workspace.MouseUp += new MouseEventHandler(pictureBox_mouseUp);
            editor_workspace.MouseMove += new MouseEventHandler(pictureBox_mouseMove);
        }
        void init_panel()
        {
            panel = new Panel();
            panel.Location = new Point(10, 25);
            panel.Size = new Size(1000, 685);
            panel.AutoScroll = true;
            panel.Controls.Add(editor_workspace);

        }
        void init_node_edit_panel()
        {
            node_edit_panel = new FlowLayoutPanel();
            node_edit_panel.BorderStyle = BorderStyle.Fixed3D;
            node_edit_panel.Size = new Size(340, 685);
            node_edit_panel.Location = new Point(1015, 25);

            init_editor_node_text_lb();
            init_editor_node_text();
            init_editor_image_name_lb();
            init_editor_image_name();
            init_editor_node_save_button();
            init_editor_node_delete_button();

            node_edit_panel.Controls.Add(editor_node_text_lb);
            node_edit_panel.Controls.Add(editor_node_text);
            node_edit_panel.Controls.Add(editor_image_name_lb);
            node_edit_panel.Controls.Add(editor_image_name);
            node_edit_panel.Controls.Add(editor_node_save);
            node_edit_panel.Controls.Add(editor_node_delete);
        }
        void init_edge_edit_panel()
        {
            edge_edit_panel = new FlowLayoutPanel();
            edge_edit_panel.BorderStyle = BorderStyle.Fixed3D;
            edge_edit_panel.Size = new Size(340, 685);
            edge_edit_panel.Location = new Point(1015, 25);

            init_editor_edge_tex_lb();
            init_editor_edge_text();
            init_editor_edge_save();
            init_editor_edge_delete();

            edge_edit_panel.Controls.Add(editor_edge_tex_lb);
            edge_edit_panel.Controls.Add(editor_edge_text);
            edge_edit_panel.Controls.Add(editor_edge_save);
            edge_edit_panel.Controls.Add(editor_edge_delete);
        }
        void init_editor_menu()
        {
            editor_menu = new MenuStrip();
            editor_menu.Dock = DockStyle.Top;
            editor_menu.LayoutStyle = ToolStripLayoutStyle.Flow;
            add_editor_menu_items();
        }
        void init_productions_print()
        {
            productions_print = new TextBox();
            productions_print.Multiline = true;
            productions_print.Location = new Point(1015, 30);
            productions_print.Size = new Size(340, 685);
            productions_print.ScrollBars = ScrollBars.Both;
        }

        void add_editor_controls_to_list()
        {
            
            editor_controls.Add(editor_workspace);
            editor_controls.Add(editor_menu);
            editor_controls.Add(node_edit_panel);
            editor_controls.Add(edge_edit_panel);
            editor_controls.Add(panel);
            editor_controls.Add(productions_print);
        }

        void change_edit_pannels()
        {
            panel.Refresh();
            if(current_node_view != null && current_edge_view == null)
            {
                node_edit_panel.Visible = true;
                edge_edit_panel.Visible = false;
            }
            else if (current_edge_view != null && current_node_view == null)
            {
                node_edit_panel.Visible = false;
                edge_edit_panel.Visible = true;
            }
            else 
            {
                node_edit_panel.Visible = false;
                edge_edit_panel.Visible = false;
            }
        }
        //-editor_menu-----------


        void add_editor_menu_items()
        {
            ToolStripButton create_node = new ToolStripButton();
            create_node.Text = "Create node";
            create_node.Click += new EventHandler(em_create_node_button_click);

            ToolStripButton show_productions = new ToolStripButton();
            show_productions.Text = "Show/hide productions";
            show_productions.Click += new EventHandler(em_productions_print_button_click);

            ToolStripDropDownButton drop_down_bt = new ToolStripDropDownButton();
            drop_down_bt.Text = "File";

            ToolStripButton to_main_menu = new ToolStripButton();
            to_main_menu.Text = "Exit";
            to_main_menu.Click += new EventHandler(em_to_main_menu_button_click);

         

            ToolStripButton delete_all = new ToolStripButton();
            delete_all.Text = "Delete all";
            delete_all.Click += new EventHandler(em_delete_all_button_click);

            ToolStripButton Load_file = new ToolStripButton(); //Добавил Радмир
            Load_file.Text = "Load file"; //Добавил Радмир
            Load_file.Click += new EventHandler(em_Load_file_button_click); // Добавил Радмир


            ToolStripButton save_file = new ToolStripButton();
            save_file.Text = "Save file";
            save_file.Click += new EventHandler(em_save_file_button_click);

            ToolStripButton seve_tree_image = new ToolStripButton();
            seve_tree_image.Text = "Save tree as image";
            seve_tree_image.Click += new EventHandler(em_save_image_tree_button_click);

            drop_down_bt.DropDown.Items.AddRange(new ToolStripItem[]
                {delete_all,to_main_menu,save_file,Load_file,seve_tree_image,show_productions});

            editor_menu.Items.Add(drop_down_bt);
            editor_menu.Items.Add(create_node);
        }

        void em_save_image_tree_button_click(object sender,EventArgs e)
        {

            var bitmap = new Bitmap(1500, 3000);
            editor_workspace.DrawToBitmap(bitmap, new Rectangle(0, 0, 1500, 3000));
            var imageFormat = System.Drawing.Imaging.ImageFormat.Png;
            bitmap.Save("TreeVisualization." + imageFormat.ToString(), imageFormat);
        }
        void em_create_node_button_click(object sender, EventArgs e)
        {
            isCreationNode = true;

        }
        void em_to_main_menu_button_click(object sender, EventArgs e)
        {
            close_graphic_editor();
            open_main_menu();
        }
        void em_delete_all_button_click(object sender, EventArgs e)
        {
            node_views.Clear();
            editor_workspace.Refresh();

        }

        // Добавил Радмир ----------------------------------------------------------
        void em_Load_file_button_click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();//Добавил Радмир
            fd.ShowDialog();                                    //Добавил Радмир
            PathOfLoad = fd.SelectedPath;                       //Добавил Радмир
            reset_graphic_editor();
            reset_id_counters();
            load_expert_system();
            start_node = nodes[0];// Добавил Радмир
            current_node = start_node; //Добавил Радмир
            create_node_views_list(); // Добавил Радмир
            create_edge_views_list(); // Добавил Радмир
            editor_workspace.Refresh();
    


        }

        // ----------------------------------------------------------------------------

        void em_save_file_button_click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();//Добавил Радмир
            fd.ShowDialog();                                    //Добавил Радмир
            PathOfSave = fd.SelectedPath;                       //Добавил Радмир

            if (questions == null)
            {
                questions = new List<Question>();
                facts = new List<Fact>();
                nodes = new List<Node>();
            }
            reset_id_counters();
            reset_exper_system();
            create_expert_system_from_view();

            save_expert_system();

        }
        void em_productions_print_button_click(object sender, EventArgs e)
        {
            if (productions_print.Visible) productions_print.Visible = false;
            else
            {
                
                productions_print.Text = "";
                string start_str = "Если ";
                print_productions_hierarchy(node_views[1], start_str);
                productions_print.Visible = true;
            }
        }

        void print_productions_hierarchy(Node_view node,string production)
        {
            if(node.edge_views.Count != 0)
            {
                production += ", и " + node.question_text + " - ";
               
               
            }
            else
            {
                production += ", то " + node.question_text;
                productions_print.AppendText(production + Environment.NewLine);
            }

            foreach(Edge_view edge in node.edge_views)
            {
                //string str = production + edge.text;
                print_productions_hierarchy(edge.destination_node, production);
            }
        }

        
        void reset_id_counters()
        {
            Node.reset_counter();
            Question.reset_counter();
            Fact.reset_counter();
        }
        void reset_exper_system()
        {
            current_node_view = null;
            current_edge_view = null;

        
            questions.Clear();
            facts.Clear();
            nodes.Clear();
        }
        void create_expert_system_from_view()
        {
            foreach (Node_view nv in node_views)
            {
                foreach (Edge_view ev in nv.edge_views)
                {
                    facts.Add(ev.get_fact());
                }
            }
            foreach (Node_view nv in node_views)
            {
                questions.Add(nv.get_qustion());
            }
            foreach (Node_view nv in node_views)
            {
                nv.init_node();
            }
            foreach (Node_view nv in node_views)
            {
                nv.init_edges();
                nodes.Add(nv.get_node());
            }

            start_node = nodes[0];
        }
        //-node_edit_panel-------
        Label editor_node_text_lb;
        TextBox editor_node_text;
        Label editor_image_name_lb;
        TextBox editor_image_name;
        Button editor_node_save;
        Button editor_node_delete;

        void init_editor_node_text_lb()
        {
            editor_node_text_lb = new Label();
            editor_node_text_lb.Text = "Qustion text:";

        }
        void init_editor_node_text()
        {
            editor_node_text = new TextBox();
            editor_node_text.Multiline = true;
            editor_node_text.Size = new Size(330, 90);
            editor_node_text.ScrollBars = ScrollBars.Both;

        }
        void init_editor_image_name_lb()
        {
            editor_image_name_lb = new Label();
            editor_image_name_lb.Text = "Image file name:";


        }
        void init_editor_image_name()
        {
            editor_image_name = new TextBox();
            editor_image_name.Size = new Size(330, 20);
            
        }
        void init_editor_node_save_button()
        {
            editor_node_save = new Button();
            editor_node_save.Text = "Save";
            editor_node_save.Click += new EventHandler(init_editor_node_save_click);
        }
        void init_editor_node_delete_button()
        {
            editor_node_delete = new Button();
            editor_node_delete.Text = "Delete";
            editor_node_delete.Click += new EventHandler(editor_node_delete_button_click);
        }

        void editor_node_delete_button_click(object sender, EventArgs e)
        {
            node_views.Remove(current_node_view);
            if (current_node_view.parent_edge != null)
            {//bad
                current_node_view.parent_edge.remove_self();
            }
            foreach(Edge_view i in current_node_view.edge_views){
                i.destination_node.parent_edge = null;
                i.destination_node.parent_node = null;
            }
            current_node_view = null;
            editor_workspace.Refresh();
            change_edit_pannels();
        }
        void init_editor_node_save_click(object sender, EventArgs e)
        {
            if (current_node_view != null)
            {
                current_node_view.question_text = editor_node_text.Text;
                current_node_view.image_name = editor_image_name.Text;

                editor_workspace.Refresh();
            }

        }

        //-edge_edit_panel-------
        Label editor_edge_tex_lb;
        TextBox editor_edge_text;
        Button editor_edge_save;
        Button editor_edge_delete;

        void init_editor_edge_tex_lb()
        {
            editor_edge_tex_lb = new Label();
            editor_edge_tex_lb.Text = "Answer text:";
        }
        void init_editor_edge_text()
        {
            editor_edge_text = new TextBox();
            editor_edge_text.Multiline = true;
            editor_edge_text.Size = new Size(330, 90);
            editor_edge_text.ScrollBars = ScrollBars.Both;

        }
        void init_editor_edge_save()
        {
            editor_edge_save = new Button();
            editor_edge_save.Text = "Save";
            editor_edge_save.Click += new EventHandler(editor_edge_save_button_click);
        }
        void init_editor_edge_delete()
        {
            editor_edge_delete = new Button();
            editor_edge_delete.Text = "Delete";
            editor_edge_delete.Click += new EventHandler(editor_edge_delete_button_click);
        }

        void editor_edge_save_button_click(object sender, EventArgs e)
        {
            current_edge_view.text = editor_edge_text.Text;
            editor_workspace.Refresh();
        }
        void editor_edge_delete_button_click(object sender, EventArgs e)
        {

            current_edge_view.remove_self();
            current_edge_view.destination_node.parent_edge = null;
            current_edge_view = null;
            editor_workspace.Refresh();
            change_edit_pannels();
        }


        void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            width = 0;
            foreach(Node_view nv in node_views)
            {
                if (nv != current_node_view)
                {
                    nv.draw(e.Graphics); 
                }

            }

            foreach (Node_view nv in node_views)
            {
                
                    foreach (Edge_view ev in nv.edge_views)
                    {
                        if (ev != current_edge_view)
                        {
                            ev.draw(e.Graphics);
                    }

                    }
                

            }
//** Этот кусок кода добавил Радмир (отрисовка Ребра)-----------------
            if (isMovingEdge) {
                Pen pen = new Pen(Color.LightGray,3);
                pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                e.Graphics.DrawLine(pen, start_cur_edge, end_cur_edge);                   
                    }
        
 //---------------------------------------------------**
            if (current_node_view != null) current_node_view.draw_highlited(e.Graphics);
           else  if (current_edge_view != null) current_edge_view.draw_heighlited(e.Graphics); 


        }

        bool isCreationEdge;
        bool isCreationNode;
        bool isMovingNode;
        bool isMovingEdge; // добавил Радмир 
        Edge_view down_edge_view;
        Node_view down_node_veiw;
        Point start_cur_edge; // добавил Радмир 
        Point end_cur_edge; // добавил Радмир 


        void pictureBox_mouseClick(object sender, MouseEventArgs e)
        {
            /*Point clickPoint = e.Location;
            foreach(Node_view nv in node_views)
            {
                if (nv.isClickedOnRectangle(clickPoint))
                {
                    current_node_view = nv;
                    current_edge_view = null;
                    editor_workspace.Refresh();
                    node_view_selected();//<-here
                    change_edit_pannels();
                    return;
                }
            }
           
            foreach (Node_view nv in node_views)
            {
                foreach (Edge_view ev in nv.edge_views)
                {
                    if (ev.isClickedOnEdge(clickPoint))
                    {
                        current_node_view = null;
                        current_edge_view = ev;
                        editor_workspace.Refresh();
                        edge_view_selected();
                        change_edit_pannels();
                        return;
                    }
                }
            }


            current_node_view = null;
            current_edge_view = null;
            change_edit_pannels();
            editor_workspace.Refresh();
            */
        }
        void pictureBox_mouseDown(object sender, MouseEventArgs e)
        {
            if (isCreationNode)
            {
                return;             
            }

            Point downPoint = e.Location;

            if (current_node_view != null) 
            {
                if (current_node_view.isClickedOnEllipse(downPoint))
                {
                    isCreationEdge = true;
                    isMovingEdge = true; // добавил Радмир
                    start_cur_edge = new Point(e.Location.X, e.Location.Y);
                    return;
                }
              
                if (current_node_view.isClickedOnRectangle(downPoint))
                {
                     
                     isMovingNode = true;
                     return;
                }              
            }
            
                foreach (Node_view nv in node_views)
                {
                    if (nv.isClickedOnRectangle(downPoint))
                    {
                        down_node_veiw = nv;
                        return;
                    }
                }
                foreach (Node_view nv in node_views)
                {
                    foreach (Edge_view ev in nv.edge_views)
                    {
                        if (ev.isClickedOnEdge(downPoint))
                        {
                            down_edge_view = ev;
                            return;
                        }
                    }
                }
            

            current_node_view = null;
            current_edge_view = null;
            change_edit_pannels();
            editor_workspace.Refresh();

        }
        void pictureBox_mouseUp(object sender, MouseEventArgs e)
        {
            Point upPoint = e.Location;
            isMovingNode = false;
            isMovingEdge = false; //Добавил Радмир 
            if (isCreationNode)
            {
                Node_view nv = new Node_view(upPoint);
                node_views.Add(nv);
                current_node_view = nv;
                current_edge_view = null;

                node_view_selected();
                change_edit_pannels();
                editor_workspace.Refresh();

                isCreationNode = false;
            }
            else
            {
                if (isCreationEdge)
                {
                    foreach(Node_view nv in node_views)
                    {
                        if (nv.isClickedOnRectangle(upPoint) && nv != current_node_view && nv.parent_edge==null)
                        {
                            Edge_view ev = new Edge_view(current_node_view, nv);
                            current_node_view.edge_views.Add(ev);
                            nv.parent_edge = ev;
                            nv.parent_node = current_node_view;
                            isCreationEdge = false;

                            current_node_view = null;
                            current_edge_view = ev;

                            edge_view_selected();
                            change_edit_pannels();
                            editor_workspace.Refresh();

                            return;
                        }
                    }
                    isCreationEdge = false;
                }
                foreach (Node_view nv in node_views)
                {
                    if (nv.isClickedOnRectangle(upPoint))
                    {
                        if(nv == down_node_veiw)
                        {

                            current_node_view = nv;
                            current_edge_view = null;
                            editor_workspace.Refresh();
                            node_view_selected();//<-here
                            change_edit_pannels();
                            return;
                        }
                    }
                }
                foreach (Node_view nv in node_views)
                {
                    foreach (Edge_view ev in nv.edge_views)
                    {
                        if (ev.isClickedOnEdge(upPoint))
                        {
                            if(ev == down_edge_view)
                            {

                                current_node_view = null;
                                current_edge_view = ev;
                                editor_workspace.Refresh();
                                edge_view_selected();
                                change_edit_pannels();
                                return;
                            }
                        }
                    }
                }


                current_node_view = null;
                current_edge_view = null;
                change_edit_pannels();
                editor_workspace.Refresh();
            }
        }
        void pictureBox_mouseMove(object sender, MouseEventArgs e)
        {
       
            if(isMovingNode||isMovingEdge)
            {
                if(isMovingNode)
                current_node_view.move(e.Location);

                if (isMovingEdge) end_cur_edge = new Point(e.Location.X, e.Location.Y); //Добавил   if (isMovingEdge&&current_edge_view!=null)   current_edge_view.update_points();


                editor_workspace.Refresh();


            }

        }

        void node_view_selected()
        {
            editor_node_text.Text = current_node_view.question_text;
            editor_image_name.Text = current_node_view.image_name;
        }
        void edge_view_selected()
        {
            editor_edge_text.Text = current_edge_view.text;
        }
  

        void create_node_views_list()
        {
            create_hierarchy(start_node, 0);
        }
        void create_edge_views_list()
        {
           foreach(Node_view nv in node_views)
            {
                 foreach(Edge e in nv.node.edges_collection.edges)
                 {
                    int dest_node_id = e.node_id;
                    Node_view dest_nv = node_views.Find(x => x.node.id == dest_node_id);
                    

                    Edge_view ev = new Edge_view(nv, dest_nv, e, facts);
                    nv.edge_views.Add(ev);
                    dest_nv.parent_edge = ev;
                    dest_nv.parent_node = nv;

                    edge_views.Add(ev);
                 }
            }
        
        }      
        void create_hierarchy(Node node,int depth)
        {
            Point location = new Point(startPoint.X + offset_X * depth, startPoint.Y + offset_Y * width);
            Node_view node_View = new Node_view(location, node, questions);
            node_views.Add(node_View);
            width += 1;

            foreach(Edge e in node.edges_collection.edges)
            {
                Node nd = nodes.Find(x => x.id == e.node_id);
                create_hierarchy(nd, depth+1);
            }
            
        }

        void close_graphic_editor()
        {
            foreach(Control contr in editor_controls)
            {
                contr.Visible = false;
            }

            this.WindowState = FormWindowState.Normal;
        }


        void open_graphic_editor()
        {
            this.WindowState = FormWindowState.Maximized;

            foreach (Control contr in editor_controls)
            {
                contr.Visible = true;
            }
            productions_print.Visible = false;
            node_views.Clear();
            width = 0;

            //create_node_views_list(); Убрал Радмир
            //create_edge_views_list(); Убрал Радмир
            editor_workspace.Refresh();

        }

        void reset_graphic_editor()
        {
            node_views.Clear();
            width = 0;
            current_edge_view = null;
            current_node_view = null;
        }


        

            //----work space-----------
        List<Control> work_space_controls;
        FlowLayoutPanel work_space;
        Label question_text;
        PictureBox question_image;
        List<RadioButton_fact> fact_buttons;
        RadioButton_fact selectedrb;
        Button answer;
        Button to_menu;
        MenuStrip menu_strip;

        bool current_node_is_answer;
        
       

        void init_work_space()
        {
            work_space = new FlowLayoutPanel();

            work_space.FlowDirection = FlowDirection.TopDown;
            
            work_space.BorderStyle = BorderStyle.Fixed3D;
            work_space.AutoScroll = true;
            work_space.Size = new Size(500, 300);
            work_space.Location = new Point(20, 30);
            work_space.BackColor = Color.Gainsboro;

            question_text = new Label();
            question_text.AutoSize = true;
            work_space.Controls.Add(question_text);

            question_image = new PictureBox();
            question_image.SizeMode = PictureBoxSizeMode.StretchImage;
            question_image.Size = new Size(320, 180);
            work_space.Controls.Add(question_image);

            current_node_is_answer = false;
            fact_buttons = new List<RadioButton_fact>(5);

            this.Controls.Add(work_space);

            init_to_menu_button();
            to_menu.Visible = false;
            this.Controls.Add(to_menu);

            init_answer_button();
            this.Controls.Add(answer);

            init_menu_strip();
            this.Controls.Add(menu_strip);

            work_space_controls = new List<Control>();
            add_controls_to_list();
        }

        void add_controls_to_list()
        {
            work_space_controls.Add(work_space);
            work_space_controls.Add(answer);
            work_space_controls.Add(to_menu);
            work_space_controls.Add(menu_strip);
        }
        void init_answer_button()
        {
            answer = new Button();
            answer.Location = new Point(430, 340);
            answer.Text = "Answer";
            answer.Click += new EventHandler(answer_button_click);
        }
        void init_to_menu_button()
        {
            to_menu = new Button();
            to_menu.Location = new Point(430, 340);
            to_menu.Text = "End";
            to_menu.Click += new EventHandler(to_menu_button_click);
        }
        void init_menu_strip()
        {
            menu_strip = new MenuStrip();
            menu_strip.Dock = DockStyle.Top;
            menu_strip.LayoutStyle = ToolStripLayoutStyle.Flow;
            add_menu_strip_items();
        }
        

        void add_menu_strip_items()
        {
            ToolStripDropDownButton drop_down_bt = new ToolStripDropDownButton();
            drop_down_bt.Text = "Menu";

            ToolStripButton ts_to_main_menu = new ToolStripButton();
            ts_to_main_menu.Text = "Exit main menu";
            ts_to_main_menu.Click += new EventHandler(ms_to_main_menu_button_click);

            ToolStripButton ts_restart = new ToolStripButton();
            ts_restart.Text = "Restart session";
            ts_restart.Click += new EventHandler(ms_restart_button_click);

            drop_down_bt.DropDown.Items.AddRange(new ToolStripItem[]
                {ts_restart,ts_to_main_menu});

            menu_strip.Items.Add(drop_down_bt);


        }

        void update_work_space()
        {
            update_qustion();
            update_facts();

            if (current_node_is_answer)
            {               
                answer.Visible = false;
                to_menu.Visible = true;
            }
            else
            {
                answer.Visible = true;
                to_menu.Visible = false;
            }
         
        }

        void update_facts()
        {
            remove_facts_from_work_space();
            fact_buttons.Clear();

            create_facts_for_work_space();
            add_facts_to_work_space();
        }
        void update_qustion()
        {
            var qst_id = current_node.question_id;
            var question = questions.Find(x => x.id == qst_id);

            question_text.Text = question.text;
            if (question.multimedia_name != null && question.multimedia_name!="")
            {
                Image image = Image.FromFile("qustion_images/" + question.multimedia_name);
                question_image.Image = image;
                question_image.BorderStyle = BorderStyle.None;
                question_image.Visible = true;
                              
            }
            else
            {
                question_image.Image = null;
                question_image.Visible = false;
            }
            
            
            
        }
        void remove_facts_from_work_space()
        {
            foreach (RadioButton i in fact_buttons)
            {
                work_space.Controls.Remove(i);
            }
            
        }
        void add_facts_to_work_space()
        {
            foreach (RadioButton i in fact_buttons)
            {
                work_space.Controls.Add(i);
            }
        }
        void create_facts_for_work_space()
        {
            int counter = 0;
            foreach (Edge i in current_node.get_edges())
            {
                var fct_id = i.fact_id;
                Fact curr_fact = facts.Find(x => x.id == fct_id);

                RadioButton_fact rb = new RadioButton_fact(counter);
                rb.Text = curr_fact.text;
                rb.AutoSize = true;
                rb.CheckedChanged += new EventHandler(radioButton_CheckedChanged);

                counter++;
                fact_buttons.Add(rb);
            }
        }
     

        void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton_fact rb = sender as RadioButton_fact;

            if (rb == null)
            {
                MessageBox.Show("Sender is not a RadioButton");
                return;
            }

            if (rb.Checked)
            {
                selectedrb = rb;
            }
        }

        void ms_restart_button_click(object sender, EventArgs e)
        {
            
            current_node = start_node;
            to_menu.Visible = false;
            current_node_is_answer = false;
            update_work_space();
        }
        void ms_to_main_menu_button_click(object sender, EventArgs e)
        {
            close_work_space();
            open_main_menu();
        }

        void answer_button_click(object sender, EventArgs e)
        {
            Button bt = sender as Button;

            if (bt == null)
            {
                MessageBox.Show("Sender is not a RadioButton");
                return;
            }

            if (selectedrb != null)
            {
                int chosed_fact_ind = selectedrb.fact_number;
                int next_node_id = current_node.get_edges()[chosed_fact_ind].node_id;

                current_node = nodes.Find(x => x.id == next_node_id);
                if (current_node.get_edges().Count == 0) current_node_is_answer = true;

                update_work_space();
                selectedrb = null;
            }

        }
        void to_menu_button_click(object sender, EventArgs e)
        {
            Button bt = sender as Button;

            if (bt == null)
            {
                MessageBox.Show("Sender is not a RadioButton");
                return;
            }

            close_work_space();
            open_main_menu();
        }

        void close_work_space()
        {
            foreach(Control control in work_space_controls)
            {
                control.Visible = false;
                
            }
            current_node = start_node;
        }
        void open_work_space()
        {
            foreach (Control control in work_space_controls)
            {
                control.Visible = true;
               
            }
            to_menu.Visible = false;
            current_node_is_answer = false;
            current_node = nodes[0];
            update_work_space();

        }

        //---saving and loading----
        Node_collection nodes_xml_serialize;
        Fact_collection facts_xml_serialize;
        Question_collection questions_xml_serialize;

        void init_save_module()
        {
            nodes_xml_serialize = new Node_collection();
            facts_xml_serialize = new Fact_collection();
            questions_xml_serialize = new Question_collection();
        }

        void load_expert_system()
        {
            load_nodes();
            load_questions();
            load_facts();
        }
        void save_expert_system()
        {
            save_nodes();
            save_facts();
            save_questions();
        }

        void load_nodes()
        {
            var xmlSerializer = new XmlSerializer(typeof(Node_collection));

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Async = true;

            FileStream fileStream = new FileStream(PathOfLoad+'/'+"nodes_xml.xml", FileMode.Open); //Было FileStream fileStream = new FileStream("nodes_xml.xml", FileMode.Open); Радмир
            var xmlReader = XmlReader.Create(fileStream, settings);

            nodes_xml_serialize = (Node_collection)xmlSerializer.Deserialize(xmlReader);
            nodes = nodes_xml_serialize.nodes;

            xmlReader.Close();
        }
        void load_facts()
        {
            var xmlSerializer = new XmlSerializer(typeof(Fact_collection));

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Async = true;

            FileStream fileStream = new FileStream(PathOfLoad + '/'+"facts_xml.xml", FileMode.Open); //Было FileStream fileStream = new FileStream("facts_xml.xml", FileMode.Open); Радмир
            var xmlReader = XmlReader.Create(fileStream, settings);

            facts_xml_serialize = (Fact_collection)xmlSerializer.Deserialize(xmlReader);
            facts = facts_xml_serialize.facts;

            xmlReader.Close();
        }
        void load_questions()
        {
            var xmlSerializer = new XmlSerializer(typeof(Question_collection));

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Async = true;

            FileStream fileStream = new FileStream(PathOfLoad + '/'+"questions_xml.xml", FileMode.Open); // Было FileStream fileStream = new FileStream("questions_xml.xml", FileMode.Open); Радмир
            var xmlReader = XmlReader.Create(fileStream, settings);

            questions_xml_serialize = (Question_collection)xmlSerializer.Deserialize(xmlReader);
            questions = questions_xml_serialize.qustions;

            xmlReader.Close();
        }

        void save_nodes()
        {
            nodes_xml_serialize.nodes = this.nodes;

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Node_collection));
            StringWriter stringWriter = new StringWriter();

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineOnAttributes = true;
            XmlWriter xmlWriter = XmlWriter.Create(PathOfSave+'/'+"nodes_xml.xml", settings); // Было XmlWriter xmlWriter = XmlWriter.Create("nodes_xml.xml", settings); Радмир


            xmlSerializer.Serialize(xmlWriter, nodes_xml_serialize);

            xmlWriter.Close();

        }
        void save_facts()
        {
            facts_xml_serialize.facts = this.facts;

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Fact_collection));
            StringWriter stringWriter = new StringWriter();

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineOnAttributes = true;
            XmlWriter xmlWriter = XmlWriter.Create(PathOfSave + '/' + "facts_xml.xml", settings); //Было XmlWriter xmlWriter = XmlWriter.Create("facts_xml.xml", settings); Радмир

            xmlSerializer.Serialize(xmlWriter, facts_xml_serialize);
            xmlWriter.Close();
        }
        void save_questions()
        {
            questions_xml_serialize.qustions = this.questions;

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Question_collection));
            StringWriter stringWriter = new StringWriter();


            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineOnAttributes = true;
            XmlWriter xmlWriter = XmlWriter.Create(PathOfSave + '/' + "questions_xml.xml", settings); //Было   XmlWriter xmlWriter = XmlWriter.Create("questions_xml.xml", settings); Радмир

            xmlSerializer.Serialize(xmlWriter, questions_xml_serialize);
            xmlWriter.Close();

        }
    }
}
