namespace Lecture33
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void btn1_Click(object sender, EventArgs e)
        {
            //Thread t1 = new(() => SetLabel("Hello From btn 1", 8000));
            //t1.Start();

            //Task.Run(() => Sum(5, 3, 8000)).ContinueWith(x =>
            //{
            //    testTxtLabel.Invoke(() =>
            //    {
            //        testTxtLabel.Text = x.Result.ToString();
            //    });
            //});

            //int result = await Task.Run(() => Sum(5, 3, 8000));
            //testTxtLabel.Text = result.ToString();

        }

        private void btn2_Click(object sender, EventArgs e)
        {
            //Thread t2 = new(() => SetLabel("Hello From btn 2", 2000));
            //t2.Start();

            //Task<int> workResult = Task.Run(() => Sum(500, 300, 8000));
            //testTxtLabel.Text = workResult.Result.ToString();
            //workResult.Wait();

        }


        //private int Sum(int x, int y, int delay = 3000)
        //{
        //    Thread.Sleep(delay);
        //    return x + y;
        //}


        //private void SetLabel(string text, int delay)
        //{
        //    Thread.Sleep(delay);

        //    testTxtLabel.Invoke(() =>
        //    {
        //        testTxtLabel.Text = text;
        //    });
        //}

    }
}
