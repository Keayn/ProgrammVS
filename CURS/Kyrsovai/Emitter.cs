using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Kyrsovai
{
    public class Emitter
    {
        public int X; // координата X центра эмиттера, будем ее использовать вместо MousePositionX
        
        public int Y; // соответствующая координата Y
        public int Direction = 0; // вектор направления в градусах куда сыпет эмиттер
        public int Spreading = 360; // разброс частиц относительно Direction
        public int SpeedMin = 1; // начальная минимальная скорость движения частицы
        public int SpeedMax = 10; // начальная максимальная скорость движения частицы
        public int RadiusMin = 2; // минимальный радиус частицы
        public int RadiusMax = 10; // максимальный радиус частицы
        public int LifeMin = 20; // минимальное время жизни частицы
        public int LifeMax = 100; // максимальное время жизни частицы
        public int ParticlesPerTick ; // количество частиц за один такт
        public int Power = 125; //Диаметр окружности
        public float GravitationX = 0;
        public float GravitationY = 1; // пусть гравитация будет силой один пиксель за такт, нам хватит

        public Color ColorFrom = Color.White; // начальный цвет частицы
        public Color ColorTo = Color.FromArgb(0, Color.Black); // конечный цвет частиц
       
        // собственно список, пока пустой
        List<Particle> particles = new List<Particle>();
        public List<Point> Krug = new List<Point>();// тту буду храниться круг
        // Метод сброса частицы
        public virtual void ResetParticle(Particle particle)
        {
            particle.Life = Particle.rand.Next(LifeMin, LifeMax);

            particle.X = X - 60;
            particle.Y = Y;

            var direction = Direction
                + (double)Particle.rand.Next(Spreading)
                - Spreading / 2;

            var speed = Particle.rand.Next(SpeedMin, SpeedMax);

            particle.SpeedX = (float)(Math.Cos(direction / 180 * Math.PI) * speed);
            particle.SpeedY = -(float)(Math.Sin(direction / 180 * Math.PI) * speed);

            particle.Radius = Particle.rand.Next(RadiusMin, RadiusMax);
        }
        // Метод генерации частиц
        public virtual Particle CreateParticle()
        {
            var particle = new ParticleColorful();
            particle.FromColor = ColorFrom;
            particle.ToColor = ColorTo;

            return particle;
        }
        // добавил функцию обновления состояния системы
       
        public void UpdateState()
        {
            int particlesToCreate = ParticlesPerTick; // фиксируем счетчик сколько частиц нам создавать за тик
           
            foreach (var particle in particles)
            {
                if (particle.Life <= 0) // если частицы умерла
                {
                    /* 
                     * то проверяем надо ли создать частицу
                     */
                    if (particlesToCreate > 0)
                    {
                        /* у нас как сброс частицы равносилен созданию частицы */
                        particlesToCreate -= 1; // поэтому уменьшаем счётчик созданных частиц на 1
                        ResetParticle(particle);
                    }
                }
                else
                {   //Гравитация

                    /*particle.SpeedX += GravitationX;
                    particle.SpeedY += GravitationY;
                    */
                    // это не трогаем
                    particle.X += particle.SpeedX;
                    particle.Y += particle.SpeedY;
                }
            }

            // второй цикл меняем на while, 
            // этот новый цикл также будет срабатывать только в самом начале работы эмиттера
            // собственно пока не накопится критическая масса частиц
            while (particlesToCreate >= 1)
            {
                particlesToCreate -= 1;
                var particle = CreateParticle();
                ResetParticle(particle);
                particles.Add(particle);
            }
        }

        public void Render(Graphics g)
        {
            // утащили сюда отрисовку частиц
            foreach (var particle in particles)
            {
                particle.Draw(g);
            }
            // рисую точки  красными кружочками
            foreach (var point in Krug)
            {
                g.DrawEllipse(
                    new Pen(Color.Red),
                    // буду рисовать окружность с диаметром равным Power
                    X - Power / 2,
                    Y - Power / 2,
                    Power,
                    Power
                );
            }
        }
    }
}
