
**The Request:**
--

 1. *The financial manager reported that when customers buy a laptop that costs less than € 500, insurance is not calculated, while it should be € 500.*
-   **Assumption/Decision Made**: Moving checking of the product type (laptop or smartphone) to add additional €500 out of “else” block in HomeController
-   **Reason**: because “else” condition is not accessible if the price of the product is less than €500, so if the product is laptop or smartphone but less than €500, the additional insurance value (€500) would not be added
-  **PR**: https://github.com/mou2ayad/InsuranceAPI/commit/409735b87fffd3d0705047ee5ef47ec720001a27
---
2. *REFACTORING*
- **Assumption/Decision Made**:  this diagram describes the new implemented flow
![Product Insurance flow](https://raw.githubusercontent.com/mou2ayad/InsuranceAPI/main/flows/task%202.png)
-   **Reason**: to make the application more maintainable and scalable, I redesigned the flow by distributing the responsibilities to multiple services.
 I moved insurance rules from the code to Insurance strategies services (insurance by product price, and insurance by product type [Laptops, Smartphones]) to a separate service that serves the insurance rules from DB, cache, or settings
I introduced a new cache level between our system and ProductAPI to cache the product and ProductType on the fly for 30 mins, with adding a toggle for this feature in the setting file (I used Distributed cache "Memcached" to consider using multi instances in Prod)   

-  **PR**: 
https://github.com/mou2ayad/InsuranceAPI/commit/0d8edc0baea0d2445036a9907967d603ac37b674

---
3. *Now we want to calculate the insurance cost for an order and for this, we are going to provide all the products that are in a shopping cart*

-   **Assumption/Decision Made**: this diagram describes the new implemented flow
- ![Order insurance Flow](https://raw.githubusercontent.com/mou2ayad/InsuranceAPI/main/flows/task%203.png)
-   **Reason**: I re-used the product insurance service from the previous task by calling it for each product in the order, I call it using multi async tasks to accelerate the process
-  **PR**: https://github.com/mou2ayad/InsuranceAPI/commit/f3331c0b40a82a16016dabbcf58d5730601c8212
---
 4. *We want to change the logic around the insurance calculation. We received a report from our business analysts that digital cameras are getting lost more than usual. Therefore, if an order has one or more digital cameras, add € 500 to the insured value of the order.*
-   **Assumption/Decision Made**: this diagram describes the new implemented flow
![Order Insurance flow with extra OrderSurchargeRate](https://raw.githubusercontent.com/mou2ayad/InsuranceAPI/main/flows/task%204.png)
-   **Reason**: I made use of the previous implementation and added one more level to get all SurchargeRate by product Type id and adding it on the total value of order insurance, considering sending multi-product having the same productType in one order to avoid adding the SurchargeRate more than once
-  **PR**: https://github.com/mou2ayad/InsuranceAPI/commit/1e3cd9b700f2af1c63fdde05ffed1b30ce0ff9fd

## The Solution consists of these Projects

 #### **1. Insurance.Utilities**
 > Class Library project (.net standard), it is a shared component, doesn't cover any business logic, practically it doesn't belong to any specific solution, it can be used in any other solution.
it provides the Error handling, Logging (NLog), CustomExceptions ,Swagger,ApiRestClient, Cache (in memory and Distributed cache "Memcached") features to any .net solution
APIRestClient contains Retry features to recall third party API with retry in case of any connectivity issue 
 
#### **2. Insurance.Storage**
> Class Library project (.net 5), it is simulation of Database (POC) so when we move to prod we replace this project with DataAceess Layer to connect to real database with Caching in between to reduce the number of same calles to the database 

 #### **3. Insurance.DataProvider**
> Class Library project (.net standard), it contains the implementation of ProductAPI client, that we use to integrate with ProductAPI and retrieve products and product types data

 #### **4. Insurance.Api**
 > RestAPI project (.net 5), it contains all the services we need to provide the product insurance values, per Product and Product Types , it exposes 2 endpoint to get insurance info per product or for all products in Shopping cart (Order)
 
 #### **5. Insurance.Tests** 
>.net Core 5 XUnit, contains Unit tests covering most parts of the project, including controller and endpoints, we essentially use Fluent Assertion, and Moq libraries, where we fake and mock the services to focus on testing specific functinalities
---
## Testing the API

 -  **Swagger URL** : https://localhost:5001/swagger/index.html
The API can be tested using Swagger directly without needing to use postman or fiddler 
 
 - **Endpoints**
 >*1- ProductInsurance Endpoint* :  To calculate insurance for Product
 GET Request
 https://localhost:5001/api/insurance/product?productId={ProductId} 
 
 
>*2- OrderInsurance Endpoint* :  To calculate insurance for Order "Shopping Cart"
 POST Request
 https://localhost:5001/api/insurance/order
```Request Body Example
{
   "contents":[
      {
         "productId":836676,
         "quantity":2
      },
      {
         "productId":859366,
         "quantity":2
      },
      {
         "productId":861866,
         "quantity":3
      }
   ]
}