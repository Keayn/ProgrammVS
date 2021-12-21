using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kyrsovai
{

    public partial class Form1 : Form
    {
        List<Emitter> emitters = new List<Emitter>();
        Emitter emitter; // добавим поле для эмиттера
        
        
        public Form1()
        {
            InitializeComponent();
            picDisplay.Image = new Bitmap(picDisplay.Width, picDisplay.Height);

            this.emitter = new Emitter // создаю эмиттер и привязываю его к полю emitter
            {
                Direction = 0,
                Spreading = 10,
                SpeedMin = 10,
                SpeedMax = 10,
                ColorFrom = Color.Gold,
                ColorTo = Color.FromArgb(0, Color.Red),
                ParticlesPerTick = 10,
                X = picDisplay.Width / 2,
                Y = picDisplay.Height / 2,
            };

            
          emitters.Add(this.emitter); // все равно добавляю в список emitters, чтобы он рендерился и обновлялся

            emitter.Krug.Add(new Point(
            picDisplay.Width / 2, picDisplay.Height / 2
                
        )) ;

        }




       
       
        
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            emitter.UpdateState(); //обновляем эмиттер

            using (var g = Graphics.FromImage(picDisplay.Image))
            {
                g.Clear(Color.Black);
                emitter.Render(g); // рендерим систему
                
            }
            

            picDisplay.Invalidate();
        }
        List<Particle> particles = new List<Particle>();

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            emitter.Direction = tbDirection.Value; // направлению эмиттера присваиваем значение ползунка 
            lblDirection.Text = $"{tbDirection.Value}°"; // добавил вывод значения
        }
        // Размер окружности
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            emitter.Power = tbPower.Value;
            lblPower.Text = $"{tbPower.Value}R";
        }
    }
    
   
}
