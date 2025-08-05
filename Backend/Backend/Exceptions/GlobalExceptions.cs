namespace Backend.Exceptions;

public static class GlobalExceptions {
public class Unauthorised() : Exception("Unauthorised");
public class UserDoesntExist() : Exception("UserDoesntExistException");
public class ProjectLimitExceeded() : Exception("ProjectLimitExceededException");
public class InvalidDate() :  Exception("InvalidDateException");

public class ProjectAlreadyAdded() : Exception("ProjectAlreadyAddedException");
} 
