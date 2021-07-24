# AppointmentBookingAPI
A simple API to book appointments.

This is a simple API, which just has one end-point to book appointmnets. 
It will accept time of appointment in 24-hour format (e.g.: 13:05) and a name.

There are certain validations for the inoput provided, which are:
- Name must be provided.
- Time must be provided, and should follow the suggested format.
- All appointmnets are 1-hour long.
- Appointments are only available between 09:00 and 17:00, which means that the last appointment availble would be at 16:00.
- There could only be 4 appointments at any given point of time, during the day. So, if you try to book a time, which is booked out, you will receive an error.

If the appointment booking is successful, then a booking id will be provided as confirmation.
Otherwise, a messgae would be displayed to inform the user of the reason, due to which the appointmnet couldn't be booked.
