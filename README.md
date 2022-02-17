# ASOS-SE-Code-Test---JamesSmith

**Notes on the design**
- As this is a library i decided not to implement try catch blocks as different client applications might want to handle errors in different ways.
- Similiar reasoning applies for logging. 
- I know the setCustomerCredit details is a blocking call but couldn't change that without making AddCustomer async which would break client code. 
- I wanted the unit tests to be illustrative of understanding rather than exhaustive given i was already over the suggested time limit. However in reality i would get as near 100% as possible as long as the tests were meaningful. 

**Next steps**

**Pratical changes**
- Use polly to improve resilliency in database calls when encountering transient problems. 
- Validate CompanyId before making database call. -- I know the goal wasn't to perfect the validation logic but this seems an easy win. 

**Happy Path/Blue Sky**
- The main issue i can see with the existing library is that there is very little feedback on the different stages of the process. 
- Exceptions can be dealt with relatively easy but for other scenarios we don't know why an addition has not succeeded (e.g. was it validation of customer, credit problem etc) as the return type is a simple boolean value.
- I can't really see how this could be corrected without changing the return type to some sort of error class that is more detailed (e.g. status code and message) which would mean changing all the client code. 
- I know this breaks the golden rule of refactoring but i would still advocate for this approach if actually working with this code as otherwise you are blind as to what is going on. 

