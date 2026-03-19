using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tips : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        // ??? если успеть фразы когда проходит 15 секунд - dont waste my time


        /** 
            первый раз когда подходишь к растению, оно говорит фразу:
            Мухоловка: "I'm hungry! Hungry!"
            Цветок: "Good morning, Darling"
            Гриб: "Z-z-z"
            Джаспер: "Hello, sir. How is your day going?"
        */

        /** 
            типы подсказок: 
            voice - текст/голос
            texture - картинка/надпись на бочке с удобрением 
            reaction - реакция растений на приближение с удобрением

            Fert 1. Гриб. Type: voice, texture.  Голос: "First one is for Lili. Hurry up. You have 1 minute". Texture: Бочка с белым крестом.
            Fert 2. Цветок. Type: texture. Бочка с надписью "BAB"
            Fert 3. Мухоловка. Type: reaction. Реакция(Flytrap): "Give it to meeeee"
            Fert 4. Цветок. Type: reaction. Реакция: "Darling, give it to me, please" Реакция(мухоловка): "No no no"
            Fert 5. Гриб. Type: texture. Бочка со знаком капля воды
            Fert 6. Мухоловка. Type: voice, reaction. Голос: "This one is for Fred." Реакция(Flower): "Darling, I hope you're not going to fertilize me with this."
            Fert 7. Мухоловка. Type: reaction. Реакция(Flytrap): "Give it to meeeee" (?)
            Fert 8. Гриб. Type: texture. Texture: Бочка с белым крестом.
            Fert 9. Цветок. Type: texture. Бочка с надписью "BAB"
            Fert 10. Джаспер. Type: voice. Голос: "Last one. Poison. You know what to do."

            + Джаспер всегда говорит, когда приходишь к нему с удобрением. Реакция: "Would you be so kind to feed me, sir?"

        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
