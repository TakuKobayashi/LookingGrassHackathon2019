import * as functions from 'firebase-functions';

// // Start writing Firebase Functions
// // https://firebase.google.com/docs/functions/typescript
//
export const sample = functions.https.onRequest((request: functions.Request, response: functions.Response) => {
  response.json({state: "success"});
});
