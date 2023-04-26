using UnityEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
//using System.Text;

namespace WorkInstructionManager
{
    /// <summary>
    /// Wrapper class that will go around the work instruction in order to achieve parity with the JSON we are
    /// getting back from the REST endpoint.
    /// </summary>

    [System.Serializable]
    public class WorkInstructionWrapper
    {
        public WorkInstruction value;
    }

    [System.Serializable]
    public class WorkInstruction
    {
        public int id;
        public string title;
        public string description;
        public List<Instruction> instructionList;

        private int index = 0;
        private readonly int max;

        /// <summary>
        /// This constructor can be used in case we want to manually create Work Instructions.
        /// Not required as we may only want to create them using JSON.
        /// </summary>
        /// <param name="inst_ID"></param>
        /// <param name="ins_Title"></param>
        /// <param name="ins_Description"></param>
        /// <param name="instructionsList"></param>
        public WorkInstruction(int inst_ID, string ins_Title, string ins_Description, List<Instruction> instructionsList)
        {
            this.id = inst_ID;
            this.title = ins_Title;
            this.description = ins_Description;
            this.instructionList = instructionsList;

            // Final instruction added for when users reach the end of a Work Instruction.
            //this.instructionList.Add(CompleteInstruction());
            this.max = instructionsList.Count - 1;
        }

        /// <summary>
        /// This can still be used if/when we get rid of the WorkInstructionWrapper (Value in the Json)
        /// by simply removing the wrapper form the class library. This works for now.
        /// </summary>
        /// <param name="json"> Json String WITH the "value" wrapper property, this can be removed later if need be.</param>
        public WorkInstruction(string json)
        {
            WorkInstructionWrapper wrapper = JsonUtility.FromJson<WorkInstructionWrapper>(json);
            WorkInstruction value = wrapper.value;

            this.id = value.id;
            this.title = value.title;
            this.description = value.description;
            this.instructionList = value.instructionList;

            // Final instruction added for when users reach the end of a Work Instruction.
            this.instructionList.Add(CompleteInstruction());
            this.max = value.instructionList.Count - 1;

        }

        /// <summary>
        /// Empty constructor for the deserializer.
        /// </summary>
        public WorkInstruction()
        {
        }

        /// <summary>
        /// Returns the first instruction.
        /// </summary>
        /// <returns>First Instruction with progress.</returns>
        public string Start()
        {
            return GetStringWithProgress();
        }

        /// <summary>
        /// Gets the next instruction, else returns current instruction.
        /// </summary>
        /// <returns>Next Instruction string.</returns>
        public string GetNextInstruction()
        {
            if (index != max)
                index++;

            return GetStringWithProgress();
        }

        /// <summary>
        /// Gets the previous instruction, else returns current instruction.
        /// </summary>
        /// <returns>Previous Instruction string.</returns>
        public string GetPrevInstruction()
        {
            if (index != 0)
                index--;

            return GetStringWithProgress();
        }

        /// <summary>
        /// Resets the user's progress back to the first instruction. 0/max.
        /// </summary>
        /// <returns>Returns the first instruction.</returns>
        public string Reset()
        {
            index = 0;
            return Start();
        }

        /// <summary>
        /// Returns a string which is composed of the current Instruction and the progress made so far as a fraction.
        /// </summary>
        /// <returns>Instruction string with progress.</returns>
        private string GetStringWithProgress()
        {
            string instruction = instructionList[index].instructionText +
                    "\n" + "(" + Progress() + ")";
            return instruction;
        }

        /// <summary>
        /// Creates a progress string in the form of a fraction.
        /// </summary>
        /// <returns></returns>
        private string Progress()
        {
            return index + "/" + max;
        }

        /// <summary>
        /// Creates an instruction that lets the User know they have completed the Work Instruction once they reach the end.
        /// </summary>
        /// <returns>Completed indicator Instruction.</returns>
        private Instruction CompleteInstruction()
        {
            return new Instruction()
            {
                instructionId = -1,
                instructionText = "COMPLETE\n\n- Click 'Reset' (I key) to start over\n- Click 'Prev' (O key) to go back a step",
                instructionImage = null,
                instructionCoordinates = null,
                instructionForeignKey = -1
            };
        }

        /// <summary>
        /// Returns a sample instruction in Json Form, this is to test without having to call the REST Endpoint.
        /// Note: The 'Complete' instruction will not be present here, and does not need to be present here.
        /// </summary>
        /// <returns>Json String Representing a Work Instruction object.</returns>
        public static string SampleWorkInstruction1()
        {
            List<string> sampleSteps = new List<string>();
            sampleSteps.Add("Get out a large bowl to mix ingridients");
            sampleSteps.Add("Place brownie mix in the bowl");
            sampleSteps.Add("Place 1/2 cup of water in the bowl");
            sampleSteps.Add("Place 1/3 cup of vegetable oil in the bowl");
            sampleSteps.Add("Crack two eggs and add them to the bowl");
            sampleSteps.Add("Mix thoroughly!");
            sampleSteps.Add("Preheat oven to 350 degrees Farenheit (or 175 degrees Celcius)");
            sampleSteps.Add("Take out a baking pan, and coat it with cooking spray, oil, or butter");
            sampleSteps.Add("Place mixed contents in the pan");
            sampleSteps.Add("Once the oven is preheated, place the pan with brownie mix in the oven for 30 Minutes.");
            sampleSteps.Add("Once 30 minutes have passed, put on baking mit");
            sampleSteps.Add("Carefully remove the brownies from oven using the mit and place in a heat resistent surface");
            sampleSteps.Add("Wait a few minutes for them to cool, cut into the shape of your choosing");
            sampleSteps.Add("Serve");
            sampleSteps.Add("ENJOY!");

            List<Instruction> sampleInstructions = new List<Instruction>();

            for (int i = 0; i < sampleSteps.Count; i++)
            {
                string imageName = "img" + i + ".png";
                string dummyCoordinates = "10,10";
                Instruction ins = new Instruction(i, sampleSteps[i], imageName, dummyCoordinates, i);
                sampleInstructions.Add(ins);
            }

            // This bypasses the 'complete' instruction addition.
            WorkInstruction sample = new WorkInstruction
            {
                id = 12345,
                title = "Make Brownies",
                description = "When making brownies, make sure you have the right ingridients!\n- Brownie Mix\n- 2x Eggs\n- 1/3 cup of Veg Oil\n- 1/2 cup of Water",
                instructionList = sampleInstructions
            };

            WorkInstructionWrapper sampleWrapper = new WorkInstructionWrapper { value = sample };
            return JsonUtility.ToJson(sampleWrapper);
        }
        
        public static string SampleWorkInstruction2()
        {
            List<string> sampleSteps = new List<string>();
            sampleSteps.Add("string");

            List<Instruction> sampleInstructions = new List<Instruction>();
            Instruction ins = new Instruction(1, "string", "string", "string", 1);
            sampleInstructions.Add(ins);

            // This bypasses the 'complete' instruction addition.
            WorkInstruction sample = new WorkInstruction
            {
                id = 1,
                title = "string",
                description = "string",
                instructionList = sampleInstructions
            };

            WorkInstructionWrapper sampleWrapper = new WorkInstructionWrapper { value = sample };
            return JsonUtility.ToJson(sampleWrapper);
        }

    }

    [System.Serializable]
    public class Instruction
    {

        public int instructionId;
        public string instructionText;
        public string instructionImage;
        public string instructionCoordinates;
        public int instructionForeignKey;

        public Instruction(int instructionId, string instructionText, string instructionImage, string instructionCoordinates, int foreignKey)
        {
            this.instructionId = instructionId;
            this.instructionText = instructionText;
            this.instructionImage = instructionImage;
            this.instructionCoordinates = instructionCoordinates;
            this.instructionForeignKey = foreignKey;
        }

        // Empty constructor for the deserializer.
        public Instruction() { }

    }

}

// TODO:
// - Instead of inserting a "COMPLETE" instruction at the end, find a more elegant solution.