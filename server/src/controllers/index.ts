import { Response, Request, Router} from  'express';

const router =  Router();

router.get('/', (req: Request, res: Response) => {
  res.json({title: 'Express'});
})

export default router;