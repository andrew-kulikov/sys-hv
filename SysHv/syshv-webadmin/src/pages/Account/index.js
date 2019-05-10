import React from 'react';
import { withNamespaces } from 'react-i18next';
import { connectTo } from '../../utils';
import Page from '../page';
import AccountDataContainer from '../../containers/AccountData';

class AccountPage extends React.Component {
  state = {
    value: 0,
    me: {}
  };

  handleChange = (event, value) => {
    this.setState({ value });
  };

  componentDidMount() {
    !this.props.token && this.props.history.replace('/');
  }

  componentDidUpdate() {
    !this.props.token && this.props.history.replace('/');
  }

  

  render() {
    const { t } = this.props;
    const { value } = this.state;
    return (
      <Page>
 
      </Page>
    );
  }
}

export default connectTo(
  state => ({
    token: state.auth.token
  }),
  {  },
  withNamespaces()(AccountPage)
);
