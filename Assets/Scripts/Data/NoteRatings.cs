public static class NoteRatings
{
    public const float RT_PERFECT = 0.075f;
    public const float RT_GREAT = 0.135f;
    public const float RT_OK = 0.25f;

    public static string ToName(this NoteRating rating)
    {
        return rating switch
        {
            NoteRating.Missed => "Missed",
            NoteRating.Ok => "Okay",
            NoteRating.Great => "Great",
            NoteRating.Perfect => "Perfect",
            _ => "NULL",
        };
    }

    public static NoteRating GetRating(float time)
    {
        if (time < RT_PERFECT)
            return NoteRating.Perfect;
        else if (time < RT_GREAT)
            return NoteRating.Great;
        else if (time < RT_OK)
            return NoteRating.Ok;

        return NoteRating.Missed;
    }
}

public enum NoteRating
{
    Missed,
    Ok,
    Great,
    Perfect,
}
