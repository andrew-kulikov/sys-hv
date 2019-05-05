import React from 'react';

import Footer from '../containers/Footer';
import Header from '../containers/Header';

const Page = props => {
  const { children } = props;
  let className = props.className;

  if (!className) className = 'HolyGrail-content';

  return (
    <div className="HolyGrail">
      <Header />

      <div className="HolyGrail-body">
        <main className={className}>{children}</main>
      </div>

      <Footer />
    </div>
  );
};

export default Page;
