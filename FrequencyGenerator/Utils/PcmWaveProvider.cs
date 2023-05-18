using System;
using NAudio.Wave;

namespace FrequencyGenerator.Utils
{
    public enum WaveType
    {
        Sine,
        Square,
        Triangle,
        Sawtooth
    }

    public class PcmWaveProvider : ISampleProvider
    {
        private readonly float[] waveTable;
        private double phase;
        private double currentPhaseStep;
        private double targetPhaseStep;
        private double frequency;
        private double phaseStepDelta;
        private bool seekFreq;

        public PcmWaveProvider(int sampleRate = 44100)
        {
            WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, 2);
            waveTable = new float[sampleRate];
            Frequency = 1000f;
            Volume = 1.0f;
            VolumeL = 1.0f;
            VolumeR = 1.0f;
            PortamentoTime = 0.5; // thought this was in seconds, but glide seems to take a bit longer
        }

        public void SetWaveType(WaveType waveType)
        {
            var sampleRate = WaveFormat.SampleRate;
            switch (waveType)
            {
                case WaveType.Sine:
                    {
                        // 生成正弦波
                        for (int index = 0; index < sampleRate; ++index)
                            waveTable[index] = (float)Math.Sin(2 * Math.PI * index / sampleRate);
                    }
                    break;
                case WaveType.Square:
                    {
                        // 生成方波
                        for (int index = 0; index < sampleRate; ++index)
                            waveTable[index] = (float)(Math.Sign(Math.Sin(2 * Math.PI * index / sampleRate))) / 2; // 缩小到 1/2 音量
                    }
                    break;
                case WaveType.Triangle:
                    {
                        // 生成三角波
                        for (int index = 0; index < sampleRate; ++index)
                            waveTable[index] = (float)(Math.Asin(Math.Sin(2 * Math.PI * index / sampleRate)));
                    }
                    break;
                case WaveType.Sawtooth:
                    {
                        // 生成锯齿波
                        for (int index = 0; index < sampleRate; ++index)
                            waveTable[index] = (float)(2 * (index / (double)sampleRate) - 1) / 2; // 缩小到 1/2 音量
                    }
                    break;
            }
        }

        public double PortamentoTime { get; set; }

        public double Frequency
        {
            get
            {
                return frequency;
            }
            set
            {
                frequency = value;
                seekFreq = true;
            }
        }

        public float Volume { get; set; }
        public float VolumeL { get; set; }
        public float VolumeR { get; set; }

        public WaveFormat WaveFormat { get; private set; }

        public int Read(float[] buffer, int offset, int count)
        {
            if (seekFreq) // process frequency change only once per call to Read
            {
                targetPhaseStep = waveTable.Length * (frequency / WaveFormat.SampleRate);

                phaseStepDelta = (targetPhaseStep - currentPhaseStep) / (WaveFormat.SampleRate * PortamentoTime);
                seekFreq = false;
            }
            // process volume change only once per call to Read
            var vol = Volume;
            var volL = VolumeL;
            var volR = VolumeR;
            for (int n = 0; n < count; n += 2)
            {
                int waveTableIndex = (int)phase % waveTable.Length;
                var sample = buffer[n + offset] = waveTable[waveTableIndex];
                buffer[n + offset] = sample * volL * vol;
                buffer[n + offset + 1] = sample * volR * vol;
                phase += currentPhaseStep;
                if (phase > waveTable.Length)
                    phase -= waveTable.Length;
                if (currentPhaseStep != targetPhaseStep)
                {
                    currentPhaseStep += phaseStepDelta;
                    if (phaseStepDelta > 0.0 && currentPhaseStep > targetPhaseStep)
                        currentPhaseStep = targetPhaseStep;
                    else if (phaseStepDelta < 0.0 && currentPhaseStep < targetPhaseStep)
                        currentPhaseStep = targetPhaseStep;
                }
            }
            return count;
        }
    }
}