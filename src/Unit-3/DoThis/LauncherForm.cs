using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Akka.Actor;
using GithubActors.Actors;

namespace GithubActors
{
    public partial class LauncherForm : Form
    {
        private IActorRef _mainFormActor;

        public LauncherForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            /* INITIALIZE ACTORS */
            _mainFormActor = Program.GithubActors.ActorOf(Props.Create(() => new MainFormActor(lblIsValid)), ActorPaths.MainFormActor.Name);
            Program.GithubActors.ActorOf(Props.Create(() => new GithubValidatorActor(GithubClientFactory.GetClient())), ActorPaths.GithubValidatorActor.Name);
            Program.GithubActors.ActorOf(Props.Create(() => new GithubCommanderActor()),
                ActorPaths.GithubCommanderActor.Name);
        }

        private void btnLaunch_Click(object sender, EventArgs e)
        {
            _mainFormActor.Tell(new ProcessRepo(tbRepoUrl.Text));
        }

        private void LauncherForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void tbRepoUrl_Clicked(object sender, EventArgs e)
        {
            var repos = new List<string>
            {
                "https://github.com/akkadotnet/akka.net",
                "https://github.com/myconstellation/constellation-packages",
                "https://github.com/boxbilling/extensions"
            };
            var rnd = new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
            tbRepoUrl.Text = repos[rnd.Next(repos.Count)];
        }
    }
}
