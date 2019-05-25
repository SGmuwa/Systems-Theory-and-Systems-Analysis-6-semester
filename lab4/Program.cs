using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab4
{
    public class BeeAlgorithm
    {
        //Алгоритм для нескольких экстремумов
        //Области рассматриваются поочерёдно, пока не выпадет максимальная точка определённое количество раз подряд
        static Random random = new Random();
        static final double EVKLID_DISTANCE = 2;//минимальное растояние между разведчиками
        static int break_parameter = 5;
        //Про пчёл
        static int scoutCount = 10;//Колличество пчёл разведчиков
        static int beeCont_toBest = 5;//Колличество пчёл, отправляемых на лучшие участки
        static int beeCount_toOption = 2;//Колличество пчёл, отправляемых на другие выбранные участки


        //Про области
        static int bestAreasCount = 2;//Колличество лучших участков
        static int selectedAreasCount = 3;//Колличество выбранных участков
        static int areaSize = 5; // Размер области для каждого участка

        //Границы поиска
        //Область D
        static int startX = -100;
        static int stopX = 100;
        static int startY = -100;
        static int stopY = 100;
        static List<Bee> TEM_CHEACK = new ArrayList<>();
        static int tem_ch_count = 0;
        static List<Bee> RES = new ArrayList<>();
        public static void main(String[] args)
        {


            List<Bee> listBee = new ArrayList<>();//Точки разведчиков
                                                  //Генерация случайных точек в области D, куда отправляются пчёлы - разведчики
            for (int i = 0; i < scoutCount; i++)
            {
                listBee.add(new Bee(random.nextInt(stopX) + startX, random.nextInt(stopY) + startY));
                double valFunc = listBee.get(i).amount_nectar();
                listBee.get(i).setValueFunc(valFunc);
                listBee.get(i).setPointCountBest(0);
            }

            //Сортировка списка для поиска лучших точек
            listBee.sort(new Comparator<Bee>() {
            @Override
            public int compare(Bee o1, Bee o2)
            {
                if (o1.getValueFunc() == o2.getValueFunc()) return 0;
                else if (o1.getValueFunc() < o2.getValueFunc()) return 1;
                else return -1;
            }
        });

        for (int i = 0; i<scoutCount; i++){
            System.out.println(listBee.get(i).x+"  "+listBee.get(i).y+"  "+listBee.get(i).valueFunc);
    }


    //Списки хранят лучшие точки и выбранные соответственно
    List<Bee> listBestArea = new ArrayList<>();//Список лучших точек (первая подобласть)
    List<Bee> listOptionArea = new ArrayList<>();//Список остальных точек (вторая подобласть)
        for (int i = 0; i<bestAreasCount; i++){
            listBestArea.add(new Bee(listBee.get(i).x, listBee.get(i).y));
        }
        for (int i = bestAreasCount; i<bestAreasCount + selectedAreasCount; i++){
            listOptionArea.add(new Bee(listBee.get(i).x, listBee.get(i).y));
        }
        for (int i = 0; i<listBestArea.size(); i++){
            listBestArea.get(i).setValueFunc(listBestArea.get(i).amount_nectar());
listBestArea.get(i).setPointCountBest(0);
        }
        for (int i = 0; i<listOptionArea.size(); i++){
            listOptionArea.get(i).setValueFunc(listOptionArea.get(i).amount_nectar());
listOptionArea.get(i).setPointCountBest(0);
        }
        
        //Считаем евклидово расстояние между точками с наибольшим значением функции и другими точками
        for(int i=0;i<listBestArea.size();i++)
            for(int j=0;j<listOptionArea.size();j++)
                if(listBestArea.get(i).distance(listOptionArea.get(i).x, listOptionArea.get(i).y)  < EVKLID_DISTANCE && listOptionArea.get(j).pointCountBest == 0 ){
                    listBestArea.get(i).setPointCountBest(listBestArea.get(i).pointCountBest+1);
listOptionArea.get(j).pointCountBest = -1;
                }
        System.out.println("listBestArea");
        for (int i = 0; i<listBestArea.size(); i++){
            System.out.println(listBestArea.get(i).x+"  "+listBestArea.get(i).y+"  "+listBestArea.get(i).valueFunc+"   "+listBestArea.get(i).pointCountBest);
        }
        System.out.println("listOptionArea");
        for (int i = 0; i<listOptionArea.size(); i++){
            System.out.println(listOptionArea.get(i).x+"  "+listOptionArea.get(i).y+"  "+listOptionArea.get(i).valueFunc+"   "+listOptionArea.get(i).pointCountBest);
        }   
        
        
        //Объединяются лучшие области и остальные выбранные в один список для удобства
        listBee.clear();
        for(int i = 0; i<listBestArea.size(); i++){
            listBee.add(listBestArea.get(i));
        }
        for (int i = 0; i<listOptionArea.size(); i++){
            if(listOptionArea.get(i).pointCountBest!= -1)
                listBee.add(listOptionArea.get(i));
        }
        System.out.println("\n"+"NEW listBee");
        for (int i = 0; i<listBee.size(); i++){
            System.out.println(listBee.get(i).x+"  "+listBee.get(i).y+"  "+listBee.get(i).valueFunc+"   "+listBee.get(i).pointCountBest);
        }
        
        List<Bee> listRes = new ArrayList<>();
        
        //Рассматриваем области
        for (int i = 0; i<listBee.size(); i++){
            int centerX = listBee.get(i).x;
int centerY = listBee.get(i).y;
System.out.println("\n"+"Область: "+i);
            do{
                List<Bee> listN = new ArrayList<>();//Пчёлы, посылаемые в область
listN.add(new Bee(centerX, centerY));
                listN.get(0).setValueFunc(listN.get(0).amount_nectar());
                listN.get(0).setPointCountBest(0);
int tempCountN;
                if(i<bestAreasCount) tempCountN =  beeCont_toBest +selectedAreasCount* listBee.get(i).pointCountBest; //Колличество пчёл, посылаемых на лучшие участки
                else tempCountN = selectedAreasCount; //Колличество пчёл, посылаемых на остальные участки
                //Определяем верхнии и нижние координаты участка (границы области)
                int lower_coordX = centerX - areaSize;
int upper_coordX = centerX + areaSize;
int lower_coordY = centerY - areaSize;
int upper_coordY = centerY + areaSize;
                //System.out.println("\n"+"lower upper                                  "+lower_coordX+ "   "+upper_coordX+"     "+lower_coordY+"     "+upper_coordY);
                for(int j = 1; j<tempCountN+1;j++){//Генерируем точки в области в колличестве пчёл, посылыемых в область
                    int x = random.nextInt(upper_coordX - lower_coordX + 1) + lower_coordX;
int y = random.nextInt(upper_coordY - lower_coordY + 1) + lower_coordY;
listN.add(new Bee(x, y));
                    listN.get(j).setValueFunc(listN.get(j).amount_nectar());
listN.get(j).setPointCountBest(0);
                }  
                
                //Сортировка списка для поиска лучших точек
                listN.sort(new Comparator<Bee>() {
                    @Override
                    public int compare(Bee o1, Bee o2)
{
    if (o1.getValueFunc() == o2.getValueFunc()) return 0;
    else if (o1.getValueFunc() < o2.getValueFunc()) return 1;
    else return -1;
}
                });
                print(listN);
boolean ch = false;
                for(int j =0;j<TEM_CHEACK.size();j++)
                    if(TEM_CHEACK.get(j).x == listN.get(0).x && TEM_CHEACK.get(j).y == listN.get(0).y ) ch=true;
                if(ch){
                    tem_ch_count++;
                    TEM_CHEACK.add(new Bee(listN.get(0).x,listN.get(0).y));
                    centerX = listN.get(0).x;
                    centerY  = listN.get(0).y;
                }
                else{
                    tem_ch_count=0;
                    TEM_CHEACK.add(new Bee(listN.get(0).x,listN.get(0).y));
                    centerX = listN.get(0).x;
                    centerY  = listN.get(0).y;
                }
                
                if(tem_ch_count>=break_parameter) listRes.add(new Bee(listN.get(0).x,listN.get(0).y));
                listN.clear();
            }while(tem_ch_count<break_parameter);
            
            TEM_CHEACK.clear();
            tem_ch_count = 0;
        }
        /*
        for(int j =0;j<TEM_CHEACK.size();j++)
                    if(TEM_CHEACK.get(j).x == listN.get(0).x && TEM_CHEACK.get(j).y == listN.get(0).y ) tem_ch_count++;
                
                if(tem_ch_count>=break_parameter) listRes.add(new Bee(listN.get(0).x,listN.get(0).y));
                else {
                    TEM_CHEACK.add(new Bee(listN.get(0).x,listN.get(0).y));
                    centerX = listN.get(0).x;
                    centerY  = listN.get(0).y;
                }
                listN.clear();*/
        
        for(int i=0;i<listRes.size();i++){
            listRes.get(i).setValueFunc(listRes.get(i).amount_nectar());
        }
        //Сортировка списка для поиска лучших точек
        listRes.sort(new Comparator<Bee>() {
        @Override
            public int compare(Bee o1, Bee o2)
{
    if (o1.getValueFunc() == o2.getValueFunc()) return 0;
    else if (o1.getValueFunc() < o2.getValueFunc()) return 1;
    else return -1;
}
        });
        System.out.println("\n"+"Варианты: ");
        for(int i=0;i<listRes.size();i++){
            System.out.println(listRes.get(i).x+"   "+listRes.get(i).y+"   "+listRes.get(i).valueFunc);
        }
         System.out.println("\n"+"Лучший вариант: ");
System.out.println(listRes.get(0).x+"   "+listRes.get(0).y+"   "+listRes.get(0).valueFunc);
    }
    private static void print(List<Bee> l)
{
    System.out.println();
    for (Bee b : l)
        System.out.println("X: " + b.x + ", Y: " + b.y + ", F: " + b.valueFunc);
}
}
}
