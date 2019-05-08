export const styles = theme => ({
  content: {
    flexGrow: 1,
    padding: theme.spacing.unit * 3,
    height: '100vh',
    overflow: 'auto'
  }
});

export const clientStyles = theme => ({
  root: {
    width: '100%'
  },
  ipHeading: {
    flexShrink: 0,
    fontSize: theme.typography.pxToRem(15),
    fontWeight: 'bold'
  },
  heading: {
    fontSize: theme.typography.pxToRem(15),
    flexShrink: 0
  },
  secondaryHeading: {
    fontSize: theme.typography.pxToRem(15),
    color: theme.palette.text.secondary
  }
});
