using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExpertSystemProject_Main
{
    class RadioButton_fact : RadioButton
    {
        public int fact_number;

        public RadioButton_fact(int number)
        {
            fact_number = number;
        }
    }

    //des_space inherited form FlowLayoutPanel, work_space. <-> this.
    class decision_space : Control
    {
        Node current_node;
        Node start_node;

        List<Question> questions; //TODO: вместо списков словарь key -- id
        List<Fact> facts;
        List<Node> nodes;


        FlowLayoutPanel work_space;
        Label question_text;
        PictureBox question_image;
        List<RadioButton_fact> fact_buttons;
        RadioButton_fact selectedrb;
        Button answer;
        Button to_menu;

        bool current_node_is_answer;

        public decision_space(Form1 mainForm)
        {
            start_node = mainForm.start_node;
            current_node = mainForm.current_node;
            questions = mainForm.questions;
            facts = mainForm.facts;
            nodes = mainForm.nodes;

            init_work_space();
            update_work_space();
        }

        void init_work_space()
        {
            work_space = new FlowLayoutPanel();

            work_space.FlowDirection = FlowDirection.TopDown;
            work_space.BorderStyle = BorderStyle.Fixed3D;
            work_space.AutoScroll = true;
            work_space.Size = new Size(500, 350);

            question_text = new Label();
            work_space.Controls.Add(question_text);

            question_image = new PictureBox();
            question_image.SizeMode = PictureBoxSizeMode.StretchImage;
            question_image.Size = new Size(300, 200);
            work_space.Controls.Add(question_image);

            current_node_is_answer = false;


            fact_buttons = new List<RadioButton_fact>(5);

            init_to_menu_button();
            to_menu.Visible = false;
            this.Controls.Add(to_menu);

            init_answer_button();
            this.Controls.Add(answer);
        }

        void init_answer_button()
        {
            answer = new Button();
            answer.Location = new Point(300, 300);
            answer.Text = "Answer";
            answer.Click += new EventHandler(answer_button_click);
        }
        void init_to_menu_button()
        {
            to_menu = new Button();
            to_menu.Location = new Point(300, 300);
            to_menu.Text = "Menu";
            to_menu.Click += new EventHandler(to_menu_button_click);
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
            if (question.multimedia_name != null)
            {
                Image image = Image.FromFile("qustion_images/" + question.multimedia_name);
                question_image.Image = image;
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
                rb.Text = curr_fact.text + "\n";
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



        }
    }
}
