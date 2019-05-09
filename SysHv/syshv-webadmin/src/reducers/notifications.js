import { createReducer } from 'redux-act';
import * as a from '../actions/notifications';

const DEFAULT_STATE = {
  notifications: JSON.parse(localStorage.getItem('notifications')) || []
};

export default createReducer(
  {
    [a.addNotification]: (state, notification) => {
      let notifications = state.notifications;
      notifications.push(notification);

      localStorage.setItem('notifications', JSON.stringify(notifications));

      return { notifications };
    },
    [a.removeNotification]: (state, id) => {
        let notifications = state.notifications;
        notifications.splice(id, 1);
        
        localStorage.setItem('notifications', JSON.stringify(notifications));
  
        return { notifications };
      }
  },
  DEFAULT_STATE
);
