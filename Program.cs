using NAudio.CoreAudioApi;
using System.Dynamic;
using System.IO.Ports;
using System.Text;

namespace AudioMeter
{
    //Audio interface for arduino (In Development) 
    //Developed by Jonny Mariani
    //GitHub: https://github.com/jonnymariani
    //Discord: @jonnymariani
    //Projected started on 22/04/2024

    //虹を見たければ、ちょっとやそっとの雨は我慢しなくちゃ 自分を信じて（じぶん を しんじて

    internal class Program
    {
        static void Main(string[] args)
        {
            //Constants
            const string PortName = "COM3";
            const int BaudRate = 19200;

            const int Multiplier = 100;

            const int AudioCheckIntervalMs = 50;

            const int AnimationFramesCheckIntervalMs = 120;

            const int StickmanHeadLineNum = 12;
            const int StickmanBodyLineNum = 13;
            const int StickmanFeetLineNum = 14;

            //const double SignalTriggerSensibility = 3.9;
            const double SignalTriggerSensibility = 5.2;

            //Keep track of current animation frame
            int currentAnimationIndex = 0;

            //Keep track of last time audio was checked
            DateTime lastAudioCheck = DateTime.Now;

            //Keep track of last time animation frame was checked
            DateTime lastAnimationFrameCheck;

            //Stickman frames

            string prefix = "|                         ";
            string sufix = "                        |";
            string marginLeft = "                       ";


            #region Gymnastics

            string GymnasticsA1 = " o     ";
            string GymnasticsA2 = "/|\\    ";
            string GymnasticsA3 = "/ \\    ";

            string GymnasticsB1 = "\\o/    ";
            string GymnasticsB2 = " |     ";
            string GymnasticsB3 = "/ \\    ";

            string GymnasticsC1 = " _ o   ";
            string GymnasticsC2 = "  /\\   ";
            string GymnasticsC3 = "/ |    ";

            string GymnasticsD1 = "       ";
            string GymnasticsD2 = " __\\o  ";
            string GymnasticsD3 = "/)  |  ";

            string GymnasticsE1 = "__|    ";
            string GymnasticsE2 = "  \\o   ";
            string GymnasticsE3 = "  ( \\  ";

            string GymnasticsF1 = "\\ /    ";
            string GymnasticsF2 = " |     ";
            string GymnasticsF3 = "/o\\    ";

            string GymnasticsG1 = "  |__  ";
            string GymnasticsG2 = "o/     ";
            string GymnasticsG3 = "/ )    ";

            string GymnasticsH1 = "       ";
            string GymnasticsH2 = "o/___  ";
            string GymnasticsH3 = " |  (\\ ";

            string GymnasticsI1 = "o _    ";
            string GymnasticsI2 = "/\\     ";
            string GymnasticsI3 = " | \\   ";

            string GymnasticsJ1 = "o _    ";
            string GymnasticsJ2 = "/\\     ";
            string GymnasticsJ3 = "/ |    ";

            #endregion

            #region Cartwheel

            string CartwheelA1 = " _o    ";
            string CartwheelA2 = "__|\\   ";
            string CartwheelA3 = "   >   ";

            string CartwheelB1 = "o/     ";
            string CartwheelB2 = "|__    ";
            string CartwheelB3 = "|      ";

            string CartwheelC1 = "  \\o_  ";
            string CartwheelC2 = "__/    ";
            string CartwheelC3 = "  >    ";

            string CartwheelD1 = "       ";
            string CartwheelD2 = "\\__/o  ";
            string CartwheelD3 = "/  \\   ";

            string CartwheelE1 = "\\ /    ";
            string CartwheelE2 = " |     ";
            string CartwheelE3 = "/o\\    ";

            string CartwheelF1 = "  __   ";
            string CartwheelF2 = " /     ";
            string CartwheelF3 = "o|     ";

            string CartwheelG1 = "__o    ";
            string CartwheelG2 = " |     ";
            string CartwheelG3 = " <<    ";

            string CartwheelH1 = " |o    ";
            string CartwheelH2 = "/      ";
            string CartwheelH3 = "|      ";

            string CartwheelI1 = "\\__/o  ";
            string CartwheelI2 = "       ";
            string CartwheelI3 = "       ";

            string CartwheelJ1 = "  |    ";
            string CartwheelJ2 = "o\\     ";
            string CartwheelJ3 = "       ";

            string CartwheelK1 = "  o____";
            string CartwheelK2 = "/      ";
            string CartwheelK3 = "       ";

            string CartwheelL1 = " _o    ";
            string CartwheelL2 = "  \\    ";
            string CartwheelL3 = "<<     ";

            string CartwheelM1 = " o/    ";
            string CartwheelM2 = " |     ";
            string CartwheelM3 = "< \\    ";

            #endregion

            #region Flip

            string FlipA1 = "  o/   ";
            string FlipA2 = "  |    ";
            string FlipA3 = " /|    ";
            string FlipB1 = "  o/   ";
            string FlipB2 = "__|    ";
            string FlipB3 = "  |    ";
            string FlipC1 = "  o__  ";
            string FlipC2 = "\\/     ";
            string FlipC3 = " |     ";
            string FlipD1 = "       ";
            string FlipD2 = "\\ __o  ";
            string FlipD3 = " |  \\  ";
            string FlipE1 = " /     ";
            string FlipE2 = "/ \\o   ";
            string FlipE3 = "   \\   ";
            string FlipF1 = "__ __  ";
            string FlipF2 = "   |o  ";
            string FlipF3 = "   |   ";
            string FlipG1 = "\\      ";
            string FlipG2 = " /\\    ";
            string FlipG3 = "|o     ";
            string FlipH1 = "       ";
            string FlipH2 = "  __|  ";
            string FlipH3 = "/o |   ";
            string FlipI1 = "|      ";
            string FlipI2 = "o\\ /   ";
            string FlipI3 = " |     ";
            string FlipJ1 = "o/     ";
            string FlipJ2 = "|__    ";
            string FlipJ3 = "|      ";
            string FlipK1 = "o/     ";
            string FlipK2 = "|      ";
            string FlipK3 = "|\\     ";

            #endregion

            #region jumping head over heals                      

            string JumpingHeadOverHealsA1 = "\\o     ";
            string JumpingHeadOverHealsA2 = " |     ";
            string JumpingHeadOverHealsA3 = " |     ";
            string JumpingHeadOverHealsB1 = "__o    ";
            string JumpingHeadOverHealsB2 = "  |    ";
            string JumpingHeadOverHealsB3 = "  |    ";
            string JumpingHeadOverHealsC1 = " o     ";
            string JumpingHeadOverHealsC2 = "/|     ";
            string JumpingHeadOverHealsC3 = "<<     ";
            string JumpingHeadOverHealsD1 = " o     ";
            string JumpingHeadOverHealsD2 = " |\\    ";
            string JumpingHeadOverHealsD3 = "<<     ";
            string JumpingHeadOverHealsE1 = "\\o     ";
            string JumpingHeadOverHealsE2 = " |     ";
            string JumpingHeadOverHealsE3 = "<<     ";
            string JumpingHeadOverHealsF1 = " |o    ";
            string JumpingHeadOverHealsF2 = " /     ";
            string JumpingHeadOverHealsF3 = "|      ";
            string JumpingHeadOverHealsG1 = "  |o   ";
            string JumpingHeadOverHealsG2 = "/|/    ";
            string JumpingHeadOverHealsG3 = "       ";
            string JumpingHeadOverHealsH1 = ">>     ";
            string JumpingHeadOverHealsH2 = " \\|o   ";
            string JumpingHeadOverHealsH3 = "       ";
            string JumpingHeadOverHealsI1 = "/\\/    ";
            string JumpingHeadOverHealsI2 = "o\\     ";
            string JumpingHeadOverHealsI3 = "       ";
            string JumpingHeadOverHealsJ1 = "o__    ";
            string JumpingHeadOverHealsJ2 = " | \\   ";
            string JumpingHeadOverHealsJ3 = "       ";
            string JumpingHeadOverHealsK1 = "_o     ";
            string JumpingHeadOverHealsK2 = " \\     ";
            string JumpingHeadOverHealsK3 = "<<     ";
            string JumpingHeadOverHealsL1 = " o/    ";
            string JumpingHeadOverHealsL2 = " |     ";
            string JumpingHeadOverHealsL3 = "< \\    ";


            #endregion

            //Create arrays responsible to animate stickman

            string[] stickTopLines =
            {
                GymnasticsA1, GymnasticsB1, GymnasticsC1, GymnasticsD1, GymnasticsE1, GymnasticsF1, GymnasticsG1, GymnasticsH1, GymnasticsI1, GymnasticsJ1,
                CartwheelA1, CartwheelB1, CartwheelC1, CartwheelD1, CartwheelE1, CartwheelF1, CartwheelG1, CartwheelH1, CartwheelI1, CartwheelJ1, CartwheelK1, CartwheelL1, CartwheelM1,
                FlipA1, FlipB1, FlipC1, FlipD1, FlipE1, FlipF1, FlipG1, FlipH1, FlipI1, FlipJ1, FlipK1,
                JumpingHeadOverHealsA1, JumpingHeadOverHealsB1, JumpingHeadOverHealsC1, JumpingHeadOverHealsD1, JumpingHeadOverHealsE1, JumpingHeadOverHealsF1, JumpingHeadOverHealsG1, JumpingHeadOverHealsH1, JumpingHeadOverHealsI1, JumpingHeadOverHealsJ1, JumpingHeadOverHealsK1, JumpingHeadOverHealsL1,
            };

            string[] stickMiddleLines =
            {
                GymnasticsA2, GymnasticsB2, GymnasticsC2, GymnasticsD2, GymnasticsE2, GymnasticsF2, GymnasticsG2, GymnasticsH2, GymnasticsI2, GymnasticsJ2,
                CartwheelA2, CartwheelB2, CartwheelC2, CartwheelD2, CartwheelE2, CartwheelF2, CartwheelG2, CartwheelH2, CartwheelI2, CartwheelJ2, CartwheelK2, CartwheelL2, CartwheelM2,
                FlipA2, FlipB2, FlipC2, FlipD2, FlipE2, FlipF2, FlipG2, FlipH2, FlipI2, FlipJ2, FlipK2,
                JumpingHeadOverHealsA2, JumpingHeadOverHealsB2, JumpingHeadOverHealsC2, JumpingHeadOverHealsD2, JumpingHeadOverHealsE2, JumpingHeadOverHealsF2, JumpingHeadOverHealsG2, JumpingHeadOverHealsH2, JumpingHeadOverHealsI2, JumpingHeadOverHealsJ2, JumpingHeadOverHealsK2, JumpingHeadOverHealsL2,
            };

            string[] stickBottomLines =
            {
                GymnasticsA3, GymnasticsB3, GymnasticsC3, GymnasticsD3, GymnasticsE3, GymnasticsF3, GymnasticsG3, GymnasticsH3, GymnasticsI3, GymnasticsJ3,
                CartwheelA3, CartwheelB3, CartwheelC3, CartwheelD3, CartwheelE3, CartwheelF3, CartwheelG3, CartwheelH3, CartwheelI3, CartwheelJ3, CartwheelK3, CartwheelL3, CartwheelM3,
                FlipA3, FlipB3, FlipC3, FlipD3, FlipE3, FlipF3, FlipG3, FlipH3, FlipI3, FlipJ3, FlipK3,
                JumpingHeadOverHealsA3, JumpingHeadOverHealsB3, JumpingHeadOverHealsC3, JumpingHeadOverHealsD3, JumpingHeadOverHealsE3, JumpingHeadOverHealsF3, JumpingHeadOverHealsG3, JumpingHeadOverHealsH3, JumpingHeadOverHealsI3, JumpingHeadOverHealsJ3, JumpingHeadOverHealsK3, JumpingHeadOverHealsL3,
            };


            #region Functions to show animation

            void StartPrint(MMDevice device)
            {
                PrintHeader();

                //Lines where stickman will be
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("");

                PrintGround(device);

                //Print first frame
                PrintNextFrame();

            }

            void PrintHeader()
            {
                PrintLine(" ________________________________________________________");
                PrintLine("|                                         -       *      |");
                PrintLine("|        *         *      -           .                  |");
                PrintLine("|                               .         .    +         |");
                PrintLine("|    __________________________________                  |");
                PrintLine("|   | __ _  _ ___ __ _     __ _  __ __ |          *      |");
                PrintLine("| * |/  / \\| \\ | /__/ \\   /  |_||_ |_  | * .            *|");
                PrintLine("|   |\\__\\_/|_/_|_\\_|\\_/   \\__| ||  |__ |     .           |");
                PrintLine("|   |__________________________________|                 |");
                PrintLine("| .  ||                              ||                  |");
                PrintLine("|--------------------------------------------------------|");
                PrintLine("|                                                    7k  |");
            }

            void PrintLine(string line)
            {
                Console.WriteLine(marginLeft + line);
            }

            void PrintGround(MMDevice device)
            {
                PrintLine("|████████████████████████████████████████████████████████|");
                PrintLine("|░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░|");
                PrintLine(" -------------------------------------------------------- ");
                PrintLine("");
                PrintLine("Connected device: " + device.FriendlyName);
            }


            //Clear current console line
            void ClearLine()
            {
                Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
            }


            void PrintNextFrame()
            {
                lastAnimationFrameCheck = DateTime.Now;

                //Print stickman current frame
                PrintStickmanHead(stickTopLines[currentAnimationIndex]);
                PrintStickmanBody(stickMiddleLines[currentAnimationIndex]);
                PrintStickmanFeet(stickBottomLines[currentAnimationIndex]);

                //If it's the last frame, reset index
                if (currentAnimationIndex + 1 == stickTopLines.Length)
                {
                    currentAnimationIndex = 1;
                }
                else
                {
                    currentAnimationIndex++;
                }
            }

            void PrintPreviousFrame()
            {
                lastAnimationFrameCheck = DateTime.Now;

                PrintStickmanHead(stickTopLines[currentAnimationIndex]);
                PrintStickmanBody(stickMiddleLines[currentAnimationIndex]);
                PrintStickmanFeet(stickBottomLines[currentAnimationIndex]);

                if (currentAnimationIndex > 0)
                {
                    currentAnimationIndex--;
                }
            }

            void PrintStickmanHead(string str)
            {
                Console.SetCursorPosition(0, StickmanHeadLineNum);
                ClearLine();
                Console.WriteLine(marginLeft + prefix + str + sufix);
            }

            void PrintStickmanBody(string str)
            {
                Console.SetCursorPosition(0, StickmanBodyLineNum);
                ClearLine();
                Console.WriteLine(marginLeft + prefix + str + sufix);
            }

            void PrintStickmanFeet(string str)
            {
                Console.SetCursorPosition(0, StickmanFeetLineNum);
                ClearLine();
                Console.WriteLine(marginLeft + prefix + str + sufix);
            }

            void PrintAudioData(float raw, float data, bool high)
            {

                Console.SetCursorPosition(0, StickmanFeetLineNum + 6);
                ClearLine();
                Console.WriteLine(marginLeft + $"Raw: {raw}");

                Console.SetCursorPosition(0, StickmanFeetLineNum + 7);
                ClearLine();
                Console.WriteLine(marginLeft + $"Analyzed: {data}");

                Console.SetCursorPosition(0, StickmanFeetLineNum + 8);
                ClearLine();
                Console.WriteLine(marginLeft + $"HIGH: {high}");
            }


            void PrintSerialData(SerialPort serialPort)
            {
                Console.SetCursorPosition(0, StickmanFeetLineNum + 9);
                ClearLine();
                Console.WriteLine(marginLeft + $"ReadBufferSize: {serialPort.ReadBufferSize}");

                Console.SetCursorPosition(0, StickmanFeetLineNum + 10);
                ClearLine();
                Console.WriteLine(marginLeft + $"WriteBufferSize: {serialPort.WriteBufferSize}");               
            }

            #endregion

            //Open serial connection
            SerialPort _serialPort = new SerialPort(PortName, BaudRate);
            _serialPort.Open();

            //Get current audio device 
            var enumerator = new MMDeviceEnumerator();
            var device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);

            //Print base data to console
            StartPrint(device);

            //While program is running
            while (true)
            {
                //_serialPort.DiscardOutBuffer();
                //_serialPort.DiscardInBuffer();

                //Get volume value
                float volume = device.AudioMeterInformation.MasterPeakValue;

                //Check time ellapse since last check
                if (DateTime.Now > lastAudioCheck.AddMilliseconds(AudioCheckIntervalMs))
                {
                    lastAudioCheck = DateTime.Now;

                    //Calculate result from volume value
                    var result = (int)Math.Floor(volume * Multiplier);

                    //Write value to serial port
                    _serialPort.WriteLine(result + "");

                    bool high = result > (Multiplier / SignalTriggerSensibility);

                    //If result is high, show dancing frame, else, show idle position
                    if (high)
                    {
                        if (DateTime.Now > lastAnimationFrameCheck.AddMilliseconds(AnimationFramesCheckIntervalMs))
                        {                            
                            PrintNextFrame();
                        }                        
                    }
                    else
                    {
                        if (DateTime.Now > lastAnimationFrameCheck.AddMilliseconds(AnimationFramesCheckIntervalMs))
                        {
                            PrintPreviousFrame();
                        }
                    }

                    PrintAudioData(volume, result, high);
                    PrintSerialData(_serialPort);

                }
            }
        }
    }
}
