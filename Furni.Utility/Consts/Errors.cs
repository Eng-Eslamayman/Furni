using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.Utility.Consts
{
    public static class Errors
    {
        public const string RequiredField = "Required Field!";
        public const string MaxLengthFor = "{PropertyName} Can not be more than {MaxLength} characters.";
        public const string MaxLength = "Length Can not be more than {1} characters.";
        public const string MaxRange = "Max Range is between {1} and {2}.";
        public const string MaxMinLength = "The {0} must be at least {2} and at max {1} characters long.";
        public const string Duplicated = "Another record with the same {0} is already exists!";
        public const string DuplicatedProduct = "Product with the same title is already exists!";
        public const string NotAllowedExtension = "Only .png, .jpg, .jpeg files are allowed!";
        public const string MaxSize = "File cannot be more than 2MB!";
        public const string NotAllowFutureDates = "Date cannot be in the future!";
        public const string InvalidRange = "{0} should be between {1} and {2}!";
        public const string ConfirmPasswordNotMatch = "The password and confirmation password do not match.";
        public const string WeakPassword = "Passwords contain an uppercase character, lowercase character, a digit, and a non-alphanumeric character. Passwords must be at least 8 characters long";
        public const string InvalidUsername = "Username can only contain letters or digits.";
        public const string OnlyEnglishLetters = "Only English letters are allowed.";
        public const string OnlyArabicLetters = "Only Arabic letters are allowed.";
        public const string OnlyNumbersAndALetters = "Only Arabic/English letters or digits are allowed.";
        public const string DenySpecialCharacters = "Special characters are not allowed.";
        public const string InvalidMobileNumber = "Invalid Mobile Number.";
        public const string InvalidNationalID = "Invalid National ID.";
        public const string InvalidSerialNumber = "Invalid serial number.";
        public const string NotAvailableRental = "This book/copy is not available for rental.";
        public const string EmptyImage = "Please Select an image.";
        public const string BlackListedSubscriber = "This subscriber is blacklisted.";
        public const string InActiveSubscriber = "This subscriber is inactive.";
        public const string MaxCopiesReached = "This subscriber has reached the max number for rentals.";
        public const string CopyIsInRental = "This copy is already rentaled.";
        public const string RentalNotAllowedForBlackListed = "Rental can not be extended for blacklisted subscribers.";
        public const string RentalNotAllowedForInActive = "Rental can not be extended for this subscriber before renewal.";
        public const string ExtendNotAllowed = "Rental can not be extended.";
        public const string PenaltyShouldBePaid = "Penalty Should Be Paid.";
        public const string InvalidStartDate = "Invalid start date.";
        public const string InvalidEndDate = "Invalid end date.";
    }
}
