using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SoLvuko
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        SodukuApi board;
        int count = 23;
        bool isGenerated = false;

        public MainPage()
        {
            InitializeComponent();
            initializeExponenet();
        }

        void initializeExponenet()
        {
            board = new SodukuApi(9);
            ShowSoduko();
        }

        private Entry GetEntry(int i ,int j)
        {
            int regionY = (int)(i - i % 3) / 3;
            int regionX = (int)(j - j % 3) / 3;
            StackLayout boardHolderObject = (StackLayout)this.FindByName("boardHolder");
            FlexLayout lineHolder = (FlexLayout)boardHolderObject.Children[regionY];
            Frame regionHolder = (Frame)lineHolder.Children[regionX];
            Frame innerHolder = (Frame)regionHolder.Content;
            StackLayout regionLineHolder = (StackLayout)innerHolder.Content;
            int indexX = j % 3, indexY = i % 3;
            FlexLayout Regionline = (FlexLayout)regionLineHolder.Children[indexY];
            Entry InputBox = (Entry)Regionline.Children[indexX];
            return InputBox;

        }

        private void ChangeState(bool isGen)
        {
            this.isGenerated = isGen;
            if (isGenerated)
            {
                var genButton = (Button)this.FindByName("GenerateSoduko");
                genButton.Text = "Solve";
            }
            else
            {
                var genButton = (Button)this.FindByName("GenerateSoduko");
                genButton.Text = "Generate";
            }
            ShowSoduko();
        }

        private void GenerateSoduko_Clicked(object sender, EventArgs e)
        {
            if (!isGenerated)
            {
                ClearBord();
                Random rnd = new Random();
                List<int> ommit = new List<int>();
                int countDown = count;
                while (countDown > 0)
                {
                    ommit.Add(rnd.Next(1, 81));
                    countDown--;
                }
                board.solve();
                int offset = 1;
                for (int i = 0; i < 9; i++, offset++)
                {
                    for (int j = 0; j < 9; j++, offset++)
                    {
                        if (ommit.Contains(offset)) board.sodukoBoard[i][j].Value = 0;
                    }
                }
                ChangeState(true);
            }
            else
            {
                board.solve();
                ChangeState(false);
            }
        }

        private void ShowSoduko()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    var entryBox = GetEntry(i, j);
                    entryBox.Text = board.sodukoBoard[i][j].isEmpty ? "" : board.sodukoBoard[i][j].Value.ToString();
                }
            }
        }

        private void ClearBord()
        {
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    GetEntry(i,j).Text = "";
            this.board = new SodukuApi(9);
        }

        private void Quit_Clicked(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            this.count = (int)e.NewValue;
            Label diffSignal = (Label)this.FindByName("Difficulty");
            if (count < 23)
            {
                diffSignal.Text = "Easy";
            }
            else if(count < 23 * 2)
            {
                diffSignal.Text = "Medium";
            }
            else
            {
                diffSignal.Text = "Hard";
            }
        }
    }
}
