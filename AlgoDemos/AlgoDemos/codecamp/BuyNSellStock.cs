using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    public class BuyNSellStock
    {
        /*
         * example - [3,3,5,0,0,3,1,4]
         * Solution beats 96% of users with C# on runtime performance (203 ms) & 16% of users with C# on Memory (50 MB)
         * I am not impressed with the code organization though. Could refactor.
         */
        public int MaxProfit(int[] prices)
        {
            int validLowest = -1, validHighest = -1, lowest = prices[0], highest = prices[0];
            int targetHighestPriceForBetterProfit = -1;
            int highestIndex = 0, lowestIndex = 0, newLowestIndex = 0;

            for (int i = 1; i < prices.Length; i++)
            {
                int currentPrice = prices[i];
                // Console.WriteLine($"currentPrice={currentPrice},validHighest={validHighest}");
                if (validLowest != -1)
                { // we already have one highest and lowest found in proper order
                    if ((targetHighestPriceForBetterProfit != -1 && currentPrice >= targetHighestPriceForBetterProfit) ||
                    (targetHighestPriceForBetterProfit == -1 && currentPrice >= validHighest))
                    {
                        validHighest = currentPrice;
                        highestIndex = i;
                        if (targetHighestPriceForBetterProfit != -1)
                        { // there was another lowest found earlier.. check if current highest can pair with that
                            if (currentPrice >= targetHighestPriceForBetterProfit)
                            {
                                validLowest = lowest;
                                lowestIndex = newLowestIndex;
                                // Console.WriteLine($"validLowest={validLowest}, lowestIndex={lowestIndex}");
                                newLowestIndex = -1;
                                lowest = -1; // reset since current 'lowest'  is set to validLowest
                                targetHighestPriceForBetterProfit = -1;
                            }
                        }
                    }
                    else if ((lowest != -1 && currentPrice < lowest) || (lowest == -1 && currentPrice < validLowest))
                    {
                        // new lowest has been found .. can be valid lowest if better buy-sell is found with higher price ahead

                        newLowestIndex = i;
                        targetHighestPriceForBetterProfit = validHighest - (validLowest - currentPrice) + 1;
                        // Console.WriteLine($"targetHighestPriceForBetterProfit={targetHighestPriceForBetterProfit},currentPrice={currentPrice}, lowest={lowest},validHighest={validHighest}");
                        lowest = currentPrice;
                    }

                }
                else
                {
                    if (currentPrice >= highest)
                    {
                        Console.WriteLine($"currentPrice {currentPrice} >=highest {highest}");
                        highest = currentPrice;
                        highestIndex = i;
                        if (highestIndex > lowestIndex)
                        { //  first proepr order found
                            validHighest = highest;
                            validLowest = lowest;
                        }
                    }
                    else if (currentPrice < lowest)
                    {
                        lowest = currentPrice;
                        lowestIndex = i;
                        if (highestIndex > lowestIndex)
                        { // first proper order found
                            validLowest = lowest;
                            validHighest = highest;
                        }
                    }
                    else if (currentPrice > lowest)
                    {
                        if (i > lowestIndex)
                        { // first proper order found
                            validHighest = currentPrice;
                            validLowest = lowest;
                            highestIndex = i;
                        }
                    }
                }
            }

            return (validHighest == -1) ? 0 : (validHighest - validLowest);
        }
    }
}
