
export function postedDateToText(postedDate){
    console.log(postedDate)
    const days = Math.floor((new Date() - new Date(postedDate)) / (1000 * 60 * 60 * 24));
    console.log(days)
    let baseString = "Posted";

    if(days == 0){
        return baseString + " Today";
    }
    else if(days >= 365){
        let years = parseInt(days / 365);
        return baseString + " " + years + (years == 1 ? " Year Ago" : " Years Ago");
    }
    else if(days >= 30){
        let months = parseInt(days / 30);
        return baseString + " " + months + (months == 1 ? " Month Ago" : " Months Ago");
    }
    else if(days >= 7){
        let weeks = parseInt(days / 7);
        return baseString + " " + weeks + (weeks == 1 ? " Week Ago" : " Weeks Ago");
    }
    else if(days >= 1){
        return baseString + " " + days + (days == 1 ? " Day Ago" : " Days Ago");
    }
    else{
        return baseString;
    }
}