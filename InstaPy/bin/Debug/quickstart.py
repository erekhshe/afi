# -*- coding: utf-8 -*-
import time
from instapy import InstaPy
from instapy.util import smart_run
from instapy import set_workspace
from instapy import get_workspace


set_workspace(path="E:\\Work\\insta Macro\\Git\\afi\\InstaPy\\bin\\Debug\\")

session = InstaPy(username='gfsdg', password='sdfgsd', headless_browser=False)

with smart_run(session):

    session.set_relationship_bounds(enabled=False, delimit_by_numbers=False)
    session.set_skip_users(skip_private=False)
    session.set_user_interact(amount=10, randomize=True, percentage=100, media='Photo')
    session.set_comments(['u'q',
                          u'w',
                          u'e',
                          u'r',
                          u't',
                          u'y'])
    session.set_do_comment(enabled=True, percentage=0)
    session.interact_user_following(['1','2','3','4','5'], amount=10, randomize=False)
    session.set_user_interact(amount=10, randomize=True, percentage=100, media='Photo')
    session.set_comments([u'q',
                          u'w',
                          u'e',
                          u'r',
                          u't',
                          u'y'])
    session.set_do_comment(enabled=True, percentage=0)
    session.interact_user_followers(['1','2','3','4','5'], amount=10, randomize=False)

    print('------------------')
    print('Finished.')
    while True:
        time.sleep(10)
