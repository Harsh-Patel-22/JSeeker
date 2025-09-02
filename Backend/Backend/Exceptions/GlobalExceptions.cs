namespace Backend.Exceptions;

public static class GlobalExceptions {
public class Unauthorised() : Exception("Unauthorised");
public class UserDoesntExist() : Exception("UserDoesntExistException");
public class ProjectLimitExceeded() : Exception("ProjectLimitExceededException");
public class InvalidDate() :  Exception("InvalidDateException");
public class AlreadyExist() : Exception("AlreadyExistException");
public class DoesNotExist() : Exception("DoesNotExistException");
public class ProjectAlreadyAdded() : Exception("ProjectAlreadyAddedException");
} 
