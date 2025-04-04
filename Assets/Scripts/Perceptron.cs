using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perceptron
{
    private float[] weights;
    private float bias;
    private float learningRate = 0.1f;

    public Perceptron(int inputCount)
    {
        weights = new float[inputCount];
        bias = Random.Range(-1f, 1f);

        for (int i = 0; i < inputCount; i++)
        {
            weights[i] = Random.Range(-1f, 1f);
        }
    }

    
    private int Activate(float sum)
    {
        return (sum >= 0) ? 1 : 0; 
    }

    
    public int Predict(float[] inputs)
    {
        float sum = bias;
        for (int i = 0; i < inputs.Length; i++)
        {
            sum += inputs[i] * weights[i];
        }
        return Activate(sum);
    }

    
    public void Train(float[] inputs, int targetOutput)
    {
        int prediction = Predict(inputs);
        int error = targetOutput - prediction;

        
        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] += learningRate * error * inputs[i];
        }
        bias += learningRate * error;
    }
}
