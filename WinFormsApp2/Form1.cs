using System.Data;
using System.Diagnostics;
using System.Drawing.Text;
using System.Globalization;
namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        const int size = 22;
        const int cellsize = 40;
        int[,,] map = new int[2, size+2, size+2];
        Button[,] buttons = new Button[size, size];
        public Form1()
        {
            InitializeComponent();
            this.Text = "Game of Life";
            Init();
            add();
        }
        private void Init()
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < size+2; j++)
                {
                    for (int k = 0; k < size+2; k++)
                    {
                        map[i, j, k] = 0;
                    }
                }
            }
            CreateMap();
            ColorMap();
        }
        private void ColorMap()
        {
            for(int i = 1; i < size+1; i++)
            {
                for(int j = 1; j < size+1; j++)
                {
                    if (map[0, i, j] == 0)
                    {
                        buttons[i-1, j-1].BackColor = Color.GhostWhite;
                    }
                    else
                    {
                        buttons[i-1, j-1].BackColor = Color.Black;
                    }
                }
            }
        }
        private void CreateMap()
        {
            this.Width = 1000;
            this.Height = 1000;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Button button = new Button();
                    button.Location = new Point(i * cellsize, j * cellsize);
                    button.Size = new Size(cellsize, cellsize);
                    button.Click += new EventHandler(onButton);
                    buttons[j, i] = button;
                    this.Controls.Add(button);
                }
            }
        }
        private void onButton(object? sender, EventArgs e)
        {
            Button Newbutton = sender as Button;
            if (map[0, Newbutton.Location.Y / cellsize + 1, Newbutton.Location.X / cellsize + 1] == 1)
            {
                buttons[Newbutton.Location.Y / cellsize, Newbutton.Location.X / cellsize].BackColor = Color.GhostWhite;
                map[0, Newbutton.Location.Y / cellsize + 1, Newbutton.Location.X / cellsize + 1] = 0;
            }
            else
            {
                if (map[0, Newbutton.Location.Y / cellsize + 1, Newbutton.Location.X / cellsize + 1] == 0)
                {
                    buttons[Newbutton.Location.Y / cellsize, Newbutton.Location.X / cellsize].BackColor = Color.Black;
                    map[0, Newbutton.Location.Y / cellsize + 1, Newbutton.Location.X / cellsize + 1] = 1;
                }
            }
        }
        private Button createbutton(int x,int y,string c)
        {
            Button button = new Button();
            button.Location = new Point(x, y);
            button.Size = new Size(100, 100);
            button.BackColor = Color.GhostWhite;
            button.Text = c;
            this.Controls.Add(button);
            return button;
        }
        private void add()
        {
            createbutton(880, 300, "Make a Move").Click+= new EventHandler(Move);
            createbutton(880, 400, "Generate").Click += new EventHandler(newRandom);
            createbutton(880, 500, "Open").Click += new EventHandler(OpenFile);
            createbutton(880, 600, "Save").Click += new EventHandler(Savetable);
        }
        private void Savetable(object? sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            string filePath = "";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    // Code to write the stream goes here.
                    filePath = saveFileDialog1.FileName;
                    myStream.Close();
                }
            }
            if (filePath == String.Empty)
                return;
            string save = "";
            for (int i = 1; i < size+1; i++)
            {
                for (int j = 1; j < size+1; j++)
                {
                    save += $"{map[0, i, j]}";
                }
                save += '\n';
            }
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine(save);
            }
        }
        private void OpenFile(object? sender, EventArgs e)
        {
            string filePath = "";
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filePath = ofd.FileName;
            }
            if (filePath == String.Empty)
                return;
            using (StreamReader read = new StreamReader(filePath))
            {
                int j = 1;
                while (read.Peek() >= 0)
                {
                    string str = read.ReadLine();//находим строку
                    if (str.Length > 0)//если длина строки больше 0
                    {
                        for (int i = 1; i < size+1; i++)
                        {
                            map[0, j, i] = Int32.Parse(str[i-1] + " ");

                        }
                    }
                    j++;
                }
            }
            ColorMap();
        }
        private void newRandom(object? sender, EventArgs e)
        {
            Random rnd = new Random();
            for(int i = 1; i < size+1; i++)
            {
                for(int j = 1; j < size+1; j++)
                {
                    int value = rnd.Next();

                    if (value % 2 == 0)
                    {
                        map[0, i, j] = 1;
                    }
                    else
                    {
                        map[0, i, j] = 0;
                    }
                }
            }
            ColorMap();
        }
        public int HowManyButton(int x, int y)
        {
            int otv = 0;
            for(int i = x - 1; i < x + 2; i++)
            {
                for(int j = y - 1; j < y + 2; j++)
                {
                    otv += map[0, i, j];
                }
            }
            return (otv - map[0,x,y]);
        }
        private void Move(object? sender, EventArgs e)
        {
            for (int i = 1; i < size+1; i++)
            {
                for (int j = 1; j < size+1; j++)
                {
                    int otv = HowManyButton(i, j);
                    if (otv == 3 && map[0,i,j]==0)
                    {
                        map[1, i, j] = 1;
                    }
                    if (otv != 3 && map[0, i, j] == 0)
                    {
                        map[1, i, j] = 0;  
                    }
                    if ((otv == 2 || otv==3) && map[0,i,j]==1)
                    {
                        map[1, i, j] = 1;
                    }
                    if ((otv != 2 && otv != 3) && map[0, i, j] == 1)
                    {
                        map[1, i, j] = 0;
                    }
                }
            }
            for (int i = 1; i < size+1; i++)
            {
                for (int j = 1; j < size+1; j++)
                {
                    map[0, i, j] = map[1, i, j];
                }
            }
            ColorMap();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            var ans = MessageBox.Show("Open File", "Are you sure?", MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (ans==DialogResult.Yes)
            {
                OpenFile(sender, e);
            }
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            var ans =  MessageBox.Show("Save table", "Are you sure?",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (ans == DialogResult.Yes)
            {
                Savetable(sender, e);
            }
        }
    }
}