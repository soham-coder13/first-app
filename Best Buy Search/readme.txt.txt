Project: Search for the best prices of your product among some of the top E-Commerce sites
Overview: This app searches for an item which is requested by the User among few of the top E-Commerce sites (Amazon,Flipkart,Paytm Mall,Snapdeal)
using website crawling algorithm.
It will then show a list of items along with the name, link of the respective item and price. The User can then click on the name to see the details
of that item from the actual site.
Technical Specification:
1. Atleast Visual Studio 2017
2. .Net Core 2.1+
3. Node version 12.x+
Version-1: Gets a list of top 20 items as per prices starting from lowest to highest.
Version-2: Added load more button that will enable users to view more items incremented by 20.
Added new loader sprinner.
Added session to store already searched items; this will enable faster search results.
Version-3: Added Snapdeal site for item search and added tasks for parallel threads.
Scope of improvement:
1. Add items from more such E-Commerce sites
2. User should have the capability to view items based on other criteria as well such as price highest to lowest.
3. Algorithm to get items' details can be made better.
4. Flipkart images cannot be retrieved at the moment, might need to be fetched from the item detail page.
5. User should have options for various filter criteria.