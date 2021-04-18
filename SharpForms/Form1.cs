using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpForms
{
    public partial class Form1 : Form
    {
        private List<GameColor> colors = new List<GameColor>();
        private SelectedGameColor[] randomColors = new SelectedGameColor[6];
        private GameColor requiredColor;
        private GameColor defaultColor = new GameColor(0, "", Color.Gray);
        private Random random = new Random();
        private int errorCounter = 0;
        private bool isGameFinished = false;
        private List<PictureBox> pictureBoxes = new List<PictureBox>();

        public Form1()
        {
            InitializeComponent();
            initializeList();
            initializePictureBoxes();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            initializeGame();
        }

        private void initializePictureBoxes()
        {
            pictureBoxes.Add(pictureBox1);
            pictureBoxes.Add(pictureBox2);
            pictureBoxes.Add(pictureBox3);
            pictureBoxes.Add(pictureBox4);
            pictureBoxes.Add(pictureBox5);
            pictureBoxes.Add(pictureBox6);
        }

        private void initializeList()
        {
            GameColor red = new GameColor(1, "красного", Color.Red);
            GameColor green = new GameColor(2, "зеленого", Color.Green);
            GameColor blue = new GameColor(3, "синего", Color.Blue);
            colors.Add(red);
            colors.Add(green);
            colors.Add(blue);
        }

        private void initializeGame()
        {
            errorCounter = 0;
            isGameFinished = false;
            setPictureBoxVisible();
            createRandomColors();
            fillPictureBoxes();
            labelText.Text = createNewText();
        }

        private void fillPictureBoxes()
        {
            for (int i = 0; i < randomColors.Length; i++)
            {
                pictureBoxes[i].BackColor = randomColors[i].Color.Color;
            }
        }

        private void createRandomColors()
        {
            requiredColor = getGameColorById(random.Next(1, 4));
            for (int i = 0; i < randomColors.Length; i++)
            {
                int randomNumber = random.Next(1, 4);
                GameColor colorByRandom = getGameColorById(randomNumber);
                randomColors[i] = new SelectedGameColor(colorByRandom);
            }
            if (!checkRandomColorsContainsRequired())
            {
                createRandomColors();
            }
        }

        private bool checkRandomColorsContainsRequired()
        {
            GameColor[] mappedColors = randomColors.Select(color => color.Color).ToArray();
            return mappedColors.Contains(requiredColor);
        }

        private GameColor getGameColorById(int id)
        {
            for (int i = 0; i < colors.Count; i++)
            {
                if (colors[i].isGameColorHasId(id))
                {
                    return colors[i];
                }
            }
            return defaultColor;
        }

        private string createNewText()
        {
            return String.Concat("Найдите все фигуры ", requiredColor.Name, " цвета");
        }

        private void setPictureBoxVisible()
        {
            pictureBoxes.ForEach(pictureBox => pictureBox.Visible = true);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            handlePickboxClick(0);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            handlePickboxClick(1);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            handlePickboxClick(2);
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            handlePickboxClick(3);

        }
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            handlePickboxClick(4);
        }
        private void pictureBox6_Click(object sender, EventArgs e)
        {
            handlePickboxClick(5);
        }

        private void handlePickboxClick(int boxIndex)
        {
            if (isGameFinished)
            {
                return;
            }
            pictureBoxes[boxIndex].Visible = false;
            randomColors[boxIndex].IsSelected = true;
            if (randomColors[boxIndex].Color != requiredColor)
            {
                errorCounter++;
            }
            isGameFinished = isAllCorrectPicturesAreSelected();
            if (isGameFinished)
            {
                handleGameEnded();
            }
        }

        private void handleGameEnded()
        {
            fillBoxesToDefault();
            switch(errorCounter)
            {
                case 0:
                    labelMark.Text = "Отлично";
                    break;
                case 1:
                    labelMark.Text = "Хорошо";
                    break;
                case 2:
                    labelMark.Text = "Удовлетворительно";
                    break;
                default:
                    labelMark.Text = "Плохо";
                    break;
            }
        }

        private void fillBoxesToDefault()
        {
            pictureBoxes.ForEach(pictureBox => pictureBox.BackColor = defaultColor.Color);
        }

        private bool isAllCorrectPicturesAreSelected()
        {
            return randomColors
                .Where(color => color.Color == requiredColor)
                .All(color => color.IsSelected);
        }

        public class SelectedGameColor
        {
            private GameColor color;
            private bool isSelected = false;

            public SelectedGameColor(GameColor color)
            {
                this.Color = color;
            }

            public GameColor Color { get => color; set => color = value; }
            public bool IsSelected { get => isSelected; set => isSelected = value; }
        }

        public class GameColor
        {
            private int id;
            private string name;
            private Color color;

            public GameColor(int id, string name, Color color)
            {
                this.Id = id;
                this.Name = name;
                this.Color = color;
            }

            public int Id { get => id; set => id = value; }
            public string Name { get => name; set => name = value; }
            public Color Color { get => color; set => color = value; }

            public override bool Equals(object obj)
            {
                return obj is GameColor color &&
                       id == color.id &&
                       name == color.name &&
                       this.color.Equals(color.color) &&
                       Id == color.Id &&
                       Name == color.Name &&
                       Color.Equals(color.Color);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(id, name, color, Id, Name, Color);
            }

            public bool isGameColorHasId(int id)
            {
                return this.Id == id;
            }
        }


    }
}
